namespace Duets.Cli.Components.Commands

open Duets.Agents
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Cli.Text.World
open Duets.Entities
open Duets.Simulation

[<RequireQualifiedAccess>]
module LookCommand =
    let private listEntrances (interactions: InteractionWithMetadata list) =
        let entrances =
            interactions
            |> List.choose (fun interaction ->
                match interaction.Interaction with
                | Interaction.FreeRoam(FreeRoamInteraction.Enter(place)) ->
                    Some(place)
                | _ -> None)

        match entrances with
        | [] -> None
        | entrances ->
            let entrancesDescription =
                Generic.listOf entrances (fun place ->
                    place.Name |> Styles.place)

            Some(
                $"""There {Generic.pluralOf "is an entrance" "are entrances" entrancesDescription.Length} towards {entrancesDescription}."""
            )
        |> Option.iter showMessage

    let private listExits (interactions: InteractionWithMetadata list) =
        let state = State.get ()

        let exits =
            interactions
            |> List.choose (fun interaction ->
                match interaction.Interaction with
                | Interaction.FreeRoam(FreeRoamInteraction.GoOut(streetId)) ->
                    Some(streetId)
                | _ -> None)

        match exits with
        | [] -> None
        | exits ->
            let exitsDescription =
                Generic.listOf exits (fun streetId ->
                    let street =
                        Queries.World.streetInCurrentCity streetId state

                    $"{street.Name |> Styles.place}")

            Some(
                $"""There is an exit towards {exitsDescription} leading out of this place."""
            )
        |> Option.iter showMessage

    let private listRoomConnections
        (interactions: InteractionWithMetadata list)
        =
        let state = State.get ()
        let cityId, placeId, _ = state |> Queries.World.currentCoordinates
        let place = Queries.World.placeInCityById cityId placeId

        let connections =
            interactions
            |> List.choose (fun interaction ->
                match interaction.Interaction with
                | Interaction.FreeRoam(FreeRoamInteraction.Move(direction,
                                                                roomId)) ->
                    let roomType = Queries.World.roomById cityId placeId roomId

                    Some(direction, roomType)
                | _ -> None)

        match connections with
        | [] -> World.noConnectionsToRoom place
        | connections -> World.connectingNodes place connections
        |> showMessage

    let private listPeople
        (knownPeople: Character list)
        (unknownPeople: Character list)
        =
        let peopleDescription people =
            Generic.listOf people (fun person ->
                $"{person.Name |> Styles.person}")

        if knownPeople.IsEmpty |> not then
            $"""{peopleDescription knownPeople} {Generic.pluralOf "is" "are" knownPeople.Length} in the room."""
            |> showMessage

        if unknownPeople.IsEmpty |> not then
            $"""There {Generic.pluralOf "is" "are" unknownPeople.Length} {unknownPeople.Length |> Styles.person} {Generic.pluralOf "person" "people" unknownPeople.Length |> Styles.person} you don't know in the room."""
            |> showMessage

    let private listItems items =
        match items with
        | [] -> Command.lookNoObjectsAround |> showMessage
        | items ->
            items
            |> List.map (fun item -> Generic.itemName item |> Items.lookItem)
            |> Items.lookItems
            |> showMessage

    let create
        (interactions: InteractionWithMetadata list)
        (items: Item list)
        (knownPeople: Character list)
        (unknownPeople: Character list)
        =
        { Name = "look"
          Description = Command.lookDescription
          Handler =
            (fun _ ->
                let state = State.get ()

                let currentPlace = state |> Queries.World.currentPlace
                let currentRoom = state |> Queries.World.currentRoom

                World.placeDescription currentPlace currentRoom.RoomType
                |> showMessage

                listRoomConnections interactions
                listEntrances interactions
                listExits interactions
                listPeople knownPeople unknownPeople
                listItems items

                Scene.World) }

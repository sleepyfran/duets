namespace Duets.Cli.Components.Commands

open Duets.Agents
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Cli.Text.LanguageModelPrompts
open Duets.Cli.Text.World
open Duets.Entities
open Duets.Simulation
open FSharp.Control

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
            |> List.concat

        match entrances with
        | [] -> None
        | entrances ->
            let entrancesDescription =
                Generic.listOf entrances World.placeNameWithType

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

    let private getConnectedStreets
        (interactions: InteractionWithMetadata list)
        =
        let connectedStreets =
            interactions
            |> List.choose (fun interaction ->
                match interaction.Interaction with
                | Interaction.FreeRoam(FreeRoamInteraction.GoToStreet(streets)) ->
                    Some(streets)
                | _ -> None)
            |> List.concat

        match connectedStreets with
        | [] -> None
        | exits ->
            Generic.listOf exits (fun street ->
                $"{street.Name |> Styles.place}")
            |> Some

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

        let connectedStreetListOpt = getConnectedStreets interactions

        match connections with
        | [] -> World.noConnectionsToRoom place connectedStreetListOpt
        | connections ->
            World.connectingNodes place connections connectedStreetListOpt
        |> showMessage

    let private listPeople
        (knownPeople: Character list)
        (unknownPeople: Character list)
        roomType
        =
        let peopleDescription people =
            Generic.listOf people (fun person ->
                $"{person.Name |> Styles.person}")

        if knownPeople.IsEmpty |> not then
            $"""{peopleDescription knownPeople} {Generic.pluralOf "is" "are" knownPeople.Length} in the {World.roomName roomType}."""
            |> showMessage

        if unknownPeople.IsEmpty |> not then
            $"""There {Generic.pluralOf "is" "are" unknownPeople.Length} {unknownPeople.Length |> Styles.person} {Generic.pluralOf "person" "people" unknownPeople.Length |> Styles.person} you don't know in the {World.roomName roomType}."""
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

                let currentRoom = state |> Queries.World.currentRoom

                createRoomDescriptionPrompt state interactions
                |> LanguageModel.streamMessage
                |> AsyncSeq.iter showInlineMessage
                |> Async.RunSynchronously

                lineBreak ()

                listRoomConnections interactions
                listEntrances interactions
                listExits interactions
                listPeople knownPeople unknownPeople currentRoom.RoomType
                listItems items

                Scene.World) }

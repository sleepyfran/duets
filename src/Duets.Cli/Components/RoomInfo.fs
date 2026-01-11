[<AutoOpen>]
module Duets.Cli.Components.RoomInfo

open Duets.Agents
open Duets.Cli.Text
open Duets.Cli.Text.World
open Duets.Entities
open Duets.Simulation

/// Lists all entrances to other places from the current room.
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

/// Lists all exits leading to streets from the current room.
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
                let street = Queries.World.streetInCurrentCity streetId state

                $"{street.Name |> Styles.place}")

        Some(
            $"""There is an exit towards {exitsDescription} leading out of this place."""
        )
    |> Option.iter showMessage

/// Gets the list of connected streets from the interactions.
let private getConnectedStreets (interactions: InteractionWithMetadata list) =
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
        Generic.listOf exits (fun street -> $"{street.Name |> Styles.place}")
        |> Some

/// Lists all room connections (directions to other rooms) from the current room.
let private listRoomConnections (interactions: InteractionWithMetadata list) =
    let state = State.get ()
    let cityId, placeId, _ = state |> Queries.World.currentCoordinates
    let place = Queries.World.placeInCityById cityId placeId

    let connections =
        interactions
        |> List.choose (fun interaction ->
            match interaction.Interaction with
            | Interaction.FreeRoam(FreeRoamInteraction.Move(direction, roomId)) ->
                let roomType = Queries.World.roomById cityId placeId roomId

                Some(direction, roomType)
            | _ -> None)

    let connectedStreetListOpt = getConnectedStreets interactions

    match connections with
    | [] -> World.noConnectionsToRoom place connectedStreetListOpt
    | connections ->
        World.connectingNodes place connections connectedStreetListOpt
    |> showMessage

/// Displays information about the current room including connections, entrances,
/// exits, people, and items. This is used both when entering a room and when
/// using the look command.
let showRoomInfo (interactions: InteractionWithMetadata list) =
    listRoomConnections interactions
    listEntrances interactions
    listExits interactions

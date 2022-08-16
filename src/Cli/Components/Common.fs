module Cli.Components.Common

open Agents
open Cli.Text
open Common
open Entities
open Simulation

/// Transforms an `EntranceError` into the correct text to show.
let showEntranceError error =
    match error with
    | EntranceError.CannotEnterBackstageOutsideConcert ->
        World.concertSpaceKickedOutOfBackstage
    | EntranceError.CannotEnterStageOutsideConcert ->
        World.concertSpaceKickedOutOfStage
    |> showMessage

/// Shows the description of the given coordinates.
let showCoordinateDescription coords =
    match coords with
    | ResolvedPlaceCoordinates coordinates ->
        match coordinates.Room with
        | RoomType.Backstage -> World.backstageDescription coordinates.Place
        | RoomType.Bar _ -> World.barDescription coordinates.Place
        | RoomType.Bedroom -> World.bedroomDescription
        | RoomType.Kitchen -> World.kitchenDescription
        | RoomType.LivingRoom -> World.livingRoomDescription
        | RoomType.Lobby -> World.lobbyDescription coordinates.Place
        | RoomType.MasteringRoom -> World.masteringRoomDescription
        | RoomType.RecordingRoom -> World.recordingRoomDescription
        | RoomType.RehearsalRoom -> World.rehearsalRoomDescription
        | RoomType.Stage -> World.stageDescription coordinates.Place
    | ResolvedOutsideCoordinates coordinates ->
        (coordinates.Node.Name, coordinates.Node.Descriptors)
        ||> match coordinates.Node.Type with
            | OutsideNodeType.Boulevard -> World.streetDescription
            | OutsideNodeType.Street -> World.boulevardDescription
            | OutsideNodeType.Square -> World.squareDescription
    |> showMessage

/// Shows all the connections to the current place based on the available
/// interactions.
let showRoomConnections interactionsWithState =
    let interactions =
        interactionsWithState
        |> List.map (fun interactionWithState ->
            interactionWithState.Interaction)

    let directions =
        interactions
        |> Interaction.chooseFreeRoam (fun interaction ->
            match interaction with
            | FreeRoamInteraction.Move (direction, nodeCoordinates) ->
                let coords =
                    Queries.World.Common.coordinates
                        (State.get ())
                        nodeCoordinates

                match coords.Content with
                | ResolvedPlaceCoordinates roomCoords ->
                    let currentPosition =
                        Queries.World.Common.currentPosition (State.get ())

                    match currentPosition.Content with
                    | ResolvedPlaceCoordinates _ ->
                        // Character is inside the place, show connected room name.
                        match roomCoords.Room with
                        | RoomType.Backstage -> World.backstageName
                        | RoomType.Bar _ -> World.barName
                        | RoomType.Bedroom -> World.bedroomName
                        | RoomType.Kitchen -> World.kitchenName
                        | RoomType.LivingRoom -> World.livingRoomName
                        | RoomType.Lobby -> World.lobbyName
                        | RoomType.MasteringRoom -> World.masteringRoomName
                        | RoomType.RecordingRoom -> World.recordingRoomName
                        | RoomType.RehearsalRoom -> World.rehearsalRoomName
                        | RoomType.Stage -> World.stageName
                    | ResolvedOutsideCoordinates _ ->
                        // Character is outside, show connected place name.
                        roomCoords.Place.Name
                | ResolvedOutsideCoordinates coords -> coords.Node.Name
                |> Tuple.two direction
                |> Some
            | _ -> None)

    if not (List.isEmpty directions) then
        directions |> Command.lookEntrances |> showMessage

    interactions
    |> Interaction.chooseFreeRoam (fun interaction ->
        match interaction with
        | FreeRoamInteraction.GoOut (_, coordinates) ->
            Some coordinates.Node.Name
        | _ -> None)
    |> List.tryHead
    |> Option.iter (Command.lookExit >> showMessage)

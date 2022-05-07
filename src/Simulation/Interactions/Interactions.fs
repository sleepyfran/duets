module Simulation.Interactions

open Entities

let private goOutFrom city destinationId =
    let cityNode =
        Queries.World.Common.contentOf city.Graph destinationId

    FreeRoamInteraction.GoOut(destinationId, cityNode)
    |> Interaction.FreeRoam

let private outsideInteractions nodeId city =
    Queries.World.Common.availableDirections nodeId city.Graph
    |> List.map (fun (direction, destinationId) ->
        FreeRoamInteraction.Move(direction, Node destinationId)
        |> Interaction.FreeRoam)

let private placeInteractions city placeId roomId place =
    let unwrappedRoomId =
        roomId
        |> Option.defaultValue place.Rooms.StartingNode

    let moveInteractions =
        Queries.World.Common.availableDirections unwrappedRoomId place.Rooms
        |> List.map (fun (direction, destinationId) ->
            FreeRoamInteraction.Move(direction, Room(placeId, destinationId))
            |> Interaction.FreeRoam)

    let exitInteraction =
        place.Exits
        |> Map.tryFind unwrappedRoomId
        |> Option.map (fun exitId -> [ goOutFrom city exitId ])
        |> Option.defaultValue []

    moveInteractions @ exitInteraction

let private concertSpaceInteractions room =
    match room with
    | ConcertSpaceRoom.Stage -> []
    | _ -> [ Interaction.FreeRoam FreeRoamInteraction.Wait ]

/// <summary>
/// Returns all interactions that are available in the current context. This
/// effectively returns all available actions in the current context that can
/// be later transformed into the actual flow.
/// </summary>
let rec availableCurrently state =
    let currentPosition =
        Queries.World.Common.currentPosition state

    let placeId, roomId =
        match currentPosition.Coordinates with
        | Room (placeId, roomId) -> placeId, Some roomId
        | Node nodeId -> nodeId, None

    let defaultInteractions =
        [ Interaction.FreeRoam FreeRoamInteraction.Wait ]

    match currentPosition.NodeContent with
    | OutsideNode _ -> outsideInteractions placeId currentPosition.City
    | ConcertPlace place ->
        let room =
            roomId
            |> Option.defaultValue place.Rooms.StartingNode
            |> Queries.World.Common.contentOf place.Rooms

        let specificInteractions =
            concertSpaceInteractions room

        placeInteractions currentPosition.City placeId roomId place
        @ specificInteractions
    | RehearsalPlace place ->
        placeInteractions currentPosition.City placeId roomId place
        @ defaultInteractions
    | StudioPlace place ->
        placeInteractions currentPosition.City placeId roomId place
        @ defaultInteractions

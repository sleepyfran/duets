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
    let moveInteractions =
        Queries.World.Common.availableDirections roomId place.Rooms
        |> List.map (fun (direction, destinationId) ->
            FreeRoamInteraction.Move(direction, Room(placeId, destinationId))
            |> Interaction.FreeRoam)

    let exitInteraction =
        place.Exits
        |> Map.tryFind roomId
        |> Option.map (fun exitId -> [ goOutFrom city exitId ])
        |> Option.defaultValue []

    moveInteractions @ exitInteraction

let private concertSpaceInteractions room =
    match room with
    | Room.Stage -> []
    | _ -> [ Interaction.FreeRoam FreeRoamInteraction.Wait ]

/// <summary>
/// Returns all interactions that are available in the current context. This
/// effectively returns all available actions in the current context that can
/// be later transformed into the actual flow.
/// </summary>
let rec availableCurrently state =
    let currentPosition =
        Queries.World.Common.currentPosition state

    let defaultInteractions =
        [ Interaction.FreeRoam FreeRoamInteraction.Wait ]

    match currentPosition.Content with
    | ResolvedPlaceCoordinates coords ->
        let placeId, roomId = coords.Coordinates

        match coords.Place.Space with
        | ConcertSpace space ->
            let specificInteractions =
                concertSpaceInteractions coords.Room

            placeInteractions currentPosition.City placeId roomId coords.Place
            @ specificInteractions
        | RehearsalSpace _
        | Studio _ ->
            placeInteractions currentPosition.City placeId roomId coords.Place
            @ defaultInteractions
    | ResolvedOutsideCoordinates coords ->
        outsideInteractions coords.Coordinates currentPosition.City
        @ defaultInteractions

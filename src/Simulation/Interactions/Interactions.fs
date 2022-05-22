module Simulation.Interactions.Root


open Entities
open Simulation

let private getOutToInteractions state destinationId =
    let cityNode =
        Queries.World.Common.coordinates state (Node destinationId)

    let outsideCoordinates =
        match cityNode.Content with
        | ResolvedOutsideCoordinates coords -> coords
        | _ -> failwith "Can't go out to a inside node"

    FreeRoamInteraction.GoOut(destinationId, outsideCoordinates)
    |> Interaction.FreeRoam

let private getOutsideInteractions nodeId city =
    Queries.World.Common.availableDirections nodeId city.Graph
    |> List.map (fun (direction, destinationId) ->
        FreeRoamInteraction.Move(direction, Node destinationId)
        |> Interaction.FreeRoam)

let private getNavigationInteractions state placeId roomId place =
    let moveInteractions =
        Queries.World.Common.availableDirections roomId place.Rooms
        |> List.map (fun (direction, destinationId) ->
            FreeRoamInteraction.Move(direction, Room(placeId, destinationId))
            |> Interaction.FreeRoam)

    let exitInteraction =
        place.Exits
        |> Map.tryFind roomId
        |> Option.map (fun exitId -> [ getOutToInteractions state exitId ])
        |> Option.defaultValue []

    moveInteractions @ exitInteraction

/// <summary>
/// Returns all interactions that are available in the current context. This
/// effectively returns all available actions in the current context that can
/// be later transformed into the actual flow.
/// </summary>
let availableCurrently state =
    let currentPosition =
        Queries.World.Common.currentPosition state

    let defaultInteractions =
        [ Interaction.FreeRoam FreeRoamInteraction.Wait
          Interaction.FreeRoam FreeRoamInteraction.Phone ]

    let allAvailableInteractions =
        match currentPosition.Content with
        | ResolvedPlaceCoordinates coords ->
            let placeId, roomId = coords.Coordinates

            let navigationInteractions =
                getNavigationInteractions state placeId roomId coords.Place

            match coords.Place.SpaceType with
            | ConcertSpace _ ->
                ConcertSpace.availableCurrently
                    state
                    coords.Room
                    navigationInteractions
                    defaultInteractions
            | RehearsalSpace _ ->
                let specificInteractions =
                    RehearsalSpace.availableCurrently state coords.Room

                navigationInteractions
                @ specificInteractions @ defaultInteractions
            | Studio studio ->
                Studio.availableCurrently state studio coords.Room
                @ navigationInteractions @ defaultInteractions
        | ResolvedOutsideCoordinates coords ->
            getOutsideInteractions coords.Coordinates currentPosition.City
            @ defaultInteractions

    allAvailableInteractions
    |> List.map (fun interaction ->
        ({ Interaction = interaction
           State = InteractionState.Enabled }))
    |> Requirements.Health.check state

module Simulation.Interactions.Root


open Entities
open Simulation

/// Result of asking for the currently available interaction.
type InteractionResult =
    { Enabled: Interaction list
      Disabled: DisabledInteraction list }

let private goOutTo state destinationId =
    let cityNode =
        Queries.World.Common.coordinates state (Node destinationId)

    let outsideCoordinates =
        match cityNode.Content with
        | ResolvedOutsideCoordinates coords -> coords
        | _ -> failwith "Can't go out to a inside node"

    FreeRoamInteraction.GoOut(destinationId, outsideCoordinates)
    |> Interaction.FreeRoam

let private outsideInteractions nodeId city =
    Queries.World.Common.availableDirections nodeId city.Graph
    |> List.map (fun (direction, destinationId) ->
        FreeRoamInteraction.Move(direction, Node destinationId)
        |> Interaction.FreeRoam)

let private placeInteractions state placeId roomId place =
    let moveInteractions =
        Queries.World.Common.availableDirections roomId place.Rooms
        |> List.map (fun (direction, destinationId) ->
            FreeRoamInteraction.Move(direction, Room(placeId, destinationId))
            |> Interaction.FreeRoam)

    let exitInteraction =
        place.Exits
        |> Map.tryFind roomId
        |> Option.map (fun exitId -> [ goOutTo state exitId ])
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

            match coords.Place.SpaceType with
            | ConcertSpace _ ->
                let specificInteractions =
                    ConcertSpace.availableCurrently state coords.Room

                placeInteractions state placeId roomId coords.Place
                @ specificInteractions
            | RehearsalSpace _ ->
                let specificInteractions =
                    RehearsalSpace.availableCurrently state coords.Room

                placeInteractions state placeId roomId coords.Place
                @ specificInteractions @ defaultInteractions
            | Studio _ ->
                placeInteractions state placeId roomId coords.Place
                @ defaultInteractions
        | ResolvedOutsideCoordinates coords ->
            outsideInteractions coords.Coordinates currentPosition.City
            @ defaultInteractions

    let enabledInteractions, disabledInteractions =
        allAvailableInteractions
        |> List.map InteractionState.Enabled
        |> Requirements.Health.check state
        |> List.fold
            (fun acc interaction ->
                let enabled, disabled = acc

                match interaction with
                | InteractionState.Enabled i -> (i :: enabled, disabled)
                | InteractionState.Disabled di -> (enabled, di :: disabled))
            ([], [])

    { Enabled = enabledInteractions
      Disabled = disabledInteractions }

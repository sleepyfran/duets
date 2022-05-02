module Simulation.Interactions

open Entities

let private placeInteractions roomId place =
    Queries.World.Common.availableDirections (Option.get roomId) place.Rooms
    |> List.map FreeRoamInteraction.Move

let private outsideInteractions nodeId city =
    Queries.World.Common.availableDirections nodeId city.Graph
    |> List.map (snd >> FreeRoamInteraction.GoOut)

let private concertSpaceInteractions room =
    match room with
    | ConcertSpaceRoom.Stage -> []
    | _ -> [ FreeRoamInteraction.Wait ]

/// <summary>
/// Returns all interactions that are available in the current context. This
/// effectively returns all available actions in the current context that can
/// be later transformed into the actual flow.
/// </summary>
let rec from state =
    let currentPosition = Queries.World.Common.currentPosition state

    let placeId, roomId =
        match currentPosition.Coordinates with
        | Room (placeId, roomId) -> placeId, Some roomId
        | Node nodeId -> nodeId, None

    let defaultInteractions = [ FreeRoamInteraction.Wait ]

    match currentPosition.NodeContent with
    | OutsideNode _ -> outsideInteractions placeId currentPosition.City
    | ConcertPlace place ->
        let room =
            Option.get roomId
            |> Queries.World.Common.contentOf place.Rooms

        let specificInteractions = concertSpaceInteractions room

        placeInteractions roomId place
        @ specificInteractions
    | RehearsalPlace place ->
        placeInteractions roomId place
        @ defaultInteractions
    | StudioPlace place ->
        placeInteractions roomId place
        @ defaultInteractions

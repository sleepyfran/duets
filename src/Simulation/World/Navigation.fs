module Simulation.World.Navigation

open Aether
open Entities
open Simulation

/// Moves to an inside location checking if the movement is possible.
let moveToInsideLocation cityId nodeId roomId =
    WorldMoveTo(cityId, nodeId, Some roomId)

/// Moves to an outside location checking if the movement is possible.
let moveToOutsideLocation cityId nodeId = WorldMoveTo(cityId, nodeId, None)

/// Moves to a location given its ID. Attempts to locate that node somewhere
/// in the world by first checking if it's part of the current city or a room
/// in the current place.
let moveTo nodeId state =
    let currentPosition = Queries.World.currentPosition state

    let node =
        Optic.get
            (Lenses.FromState.World.node_ currentPosition.City.Id nodeId)
            state

    match node with
    | Some _ -> moveToOutsideLocation currentPosition.City.Id nodeId
    | None ->
        moveToInsideLocation
            currentPosition.City.Id
            currentPosition.NodeId
            nodeId

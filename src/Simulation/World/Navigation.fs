module Simulation.World.Navigation

open Aether
open Entities
open Simulation

/// Moves the player to the specific node ID.
let moveTo nodeId state =
    let currentPosition = Queries.World.currentPosition state

    WorldMoveTo(currentPosition.City.Id, nodeId)

module Simulation.State.World

open Aether
open Entities

let move cityId nodeId =
    Optic.set Lenses.State.currentPosition_ (cityId, nodeId)

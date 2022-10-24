module Simulation.State.World

open Aether
open Entities

let move cityId placeId =
    Optic.set Lenses.State.currentPosition_ (cityId, placeId)

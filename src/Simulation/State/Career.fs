module Simulation.State.Career

open Aether
open Entities

let set job =
    Optic.set Lenses.State.career_ job

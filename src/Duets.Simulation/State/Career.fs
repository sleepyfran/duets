module Duets.Simulation.State.Career

open Aether
open Duets.Entities

let set job = Optic.set Lenses.State.career_ job

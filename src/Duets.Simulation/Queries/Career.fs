module Duets.Simulation.Queries.Career

open Aether
open Duets.Entities

/// Returns the current career in which the character works, if any.
let current = Optic.get Lenses.State.career_

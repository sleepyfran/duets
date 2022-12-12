module Simulation.Queries.Career

open Aether
open Entities

/// Returns the current career in which the character works, if any.
let current = Optic.get Lenses.State.career_

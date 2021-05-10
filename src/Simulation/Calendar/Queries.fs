module Simulation.Calendar.Queries

open Aether
open Entities
open Storage

/// Returns the current date in game.
let today state = state |> Optic.get Lenses.State.today_

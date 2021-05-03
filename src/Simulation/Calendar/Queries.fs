module Simulation.Calendar.Queries

open Entities
open Lenses
open Storage

/// Returns the current date in game.
let today () =
  State.getState () |> Lens.get StateLenses.Today

module Simulation.Bands.Queries

open Storage.State

/// Returns the currently selected band in the game.
let currentBand () =
  getState ()
  |> fun state -> state.Bands
  |> List.head
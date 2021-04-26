module Simulation.Setup

open Entities
open Entities.State
open Storage.State

/// Sets up the initial game state based on the data provided by the user in
/// the setup wizard.
let startGame character band =
  { Character = character
    CharacterSkills = Map.empty
    Bands = [ band ]
    UnfinishedSongs = Map.empty
    Today = Calendar.fromDayMonth 1 1 }
  |> setState

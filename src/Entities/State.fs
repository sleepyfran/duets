module Entities.State

open Entities.Band
open Entities.Calendar
open Entities.Character

/// Shared state of the game. Contains all state that is common to every part
/// of the game.
type State =
  { Character: Character
    Bands: Band list
    Today: Date }

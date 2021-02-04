module Entities.State

open Entities.Band
open Entities.Character

/// Shared state of the game. Contains all state that is common to every part
/// of the game.
type State =
  { Character: Character
    Band: Band
    Today: Calendar.Date }

  static member Character_ =
    (fun w -> w.Character), (fun c w -> { w with Character = c })

  static member Band_ =
    (fun w -> w.Band), (fun b w -> { w with Band = b })

  static member Today_ =
    (fun w -> w.Today), (fun t w -> { w with Today = t })

/// Creates a default world with only the character set. The rest of
/// the members will get default new game values.
let createFor character =
  { Character = character
    Band = Band.getDefault ()
    Today = Calendar.fromDayMonth 1 1 }

/// Creates an empty state for testing purposes only. TODO: Remove once not needed.
let empty () =
  { Character = Character.getDefault ()
    Band = Band.getDefault ()
    Today = Calendar.fromDayMonth 1 1 }

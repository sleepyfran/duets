module Startup

open View.Actions
open View.Scenes.Index
open Entities.State

/// Given a savegame file, returns the current state of the game as well as the
/// the corresponding screen.
let fromSavegame savegame =
  match savegame with
  | Some (_) -> (empty (), Scene MainMenu)
  | None -> (empty (), Scene MainMenu)

module Startup

open View.Scenes.MainMenu
open Entities.State

/// Given a savegame file, returns the current state of the game as well as the
/// the corresponding screen.
let fromSavegame savegame =
  match savegame with
  | Some (_) -> (empty (), mainMenu ())
  | None -> (empty (), mainMenu ())

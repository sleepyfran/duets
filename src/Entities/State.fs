module Entities.State

open Entities.Band
open Entities.Calendar
open Entities.Character
open Entities.Song

/// Shared state of the game. Contains all state that is common to every part
/// of the game.
type State =
  { Character: Character
    Band: Band
    UnfinishedSongs: Map<BandId, Song list>
    FinishedSongs: Map<BandId, Song list>
    Today: Date }

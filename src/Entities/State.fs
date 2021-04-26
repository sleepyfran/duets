module Entities.State

open Entities.Calendar

/// Creates an empty state with all the fields set to its empty representation.
let empty =
  { Bands = List.empty
    Character = Character.empty
    CharacterSkills = Map.empty
    UnfinishedSongs = Map.empty
    Today = fromDayMonth 1 1 }

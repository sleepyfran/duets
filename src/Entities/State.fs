module Entities.State

open Entities

/// Creates an empty state with all the fields set to its empty representation.
let empty =
  { Bands = List.empty
    Character = Character.empty
    CharacterSkills = Map.empty
    BandRepertoire = Band.Repertoire.empty
    Today = Calendar.fromDayMonth 1 1 }

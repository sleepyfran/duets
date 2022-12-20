module Data.Skills

open Entities

/// Generates all the available skills in the game, scoped to a given genre and
/// instrument.
let allFor (genre: Genre) (instrument: InstrumentType) =
    [ SkillId.Composition
      SkillId.Genre genre
      SkillId.Instrument instrument
      SkillId.MusicProduction
      SkillId.Speech ]
    |> List.map Skill.create

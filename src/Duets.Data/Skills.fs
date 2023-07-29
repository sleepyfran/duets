module Duets.Data.Skills

open Duets.Entities

/// Generates all the available skills in the game, scoped to a given genre and
/// instrument.
let allFor (genre: Genre) (instrument: InstrumentType) =
    [ // Music.
      SkillId.Composition
      SkillId.Genre genre
      SkillId.Instrument instrument
      // Production.
      SkillId.MusicProduction
      // Character.
      SkillId.Speech
      // Job.
      SkillId.Barista
      SkillId.Bartending ]
    |> List.map Skill.create

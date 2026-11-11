module Duets.Data.Skills

open Duets.Common
open Duets.Entities

/// Generates all the available skills in the game, scoped to a given genre and
/// instruments.
let allFor (genre: Genre) (instruments: InstrumentType list) =
    let skillsWithoutParams = Union.allCasesOf<SkillId> ()

    SkillId.Genre genre
    :: (instruments |> List.map SkillId.Instrument)
    @ skillsWithoutParams
    |> List.map Skill.create

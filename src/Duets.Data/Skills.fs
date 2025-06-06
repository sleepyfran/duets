module Duets.Data.Skills

open Duets.Common
open Duets.Entities

/// Generates all the available skills in the game, scoped to a given genre and
/// instrument.
let allFor (genre: Genre) (instrument: InstrumentType) =
    let skillsWithoutParams = Union.allCasesOf<SkillId> ()

    SkillId.Genre genre :: SkillId.Instrument instrument :: skillsWithoutParams
    |> List.map Skill.create

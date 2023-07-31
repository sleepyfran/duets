module Duets.Simulation.Skills.Improve.Career

open Duets.Entities
open Duets.Simulation

/// Grants a 25% chance of improving the skills associated with the current
/// stage of a job.
let improveCharacterSkillsAfterShift job state =
    let character = Queries.Characters.playableCharacter state
    let skillIds = Career.jobSkills job

    Common.applySkillModificationChance
        state
        {| CharacterId = character.Id
           Skills = skillIds
           Chance = 25
           ImprovementAmount = 1 |}

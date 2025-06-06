module Duets.Simulation.Careers.RequirementCharacterUpgrade

open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Skills.Improve.Common

/// This funkily named function takes care of applying a 25% chance of
/// the requirements for the next stage of the job improving, meaning skills
/// and fame where applicable.
let rec applyRequirementUpgradeChange job state =
    job.CurrentStage.Requirements
    |> List.collect (function
        | CareerStageRequirement.Skill(skill, _) ->
            improveCharacterSkillAfterShift skill state
        | CareerStageRequirement.Fame _ -> improveCharacterFameAfterShift state)

and private improveCharacterFameAfterShift state =
    let character = Queries.Characters.playableCharacter state
    let chanceAwarded = RandomGen.chance 25

    Character.Attribute.conditionalAdd
        character
        CharacterAttribute.Fame
        chanceAwarded
        1

and private improveCharacterSkillAfterShift skill state =
    let character = Queries.Characters.playableCharacter state

    applySkillModificationChance
        state
        {| CharacterId = character.Id
           Skills = [ skill ]
           Chance = 25
           ImprovementAmount = 1 |}

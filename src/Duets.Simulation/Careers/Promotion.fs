module Duets.Simulation.Careers.Promotion

open Duets.Entities
open Duets.Data
open Duets.Simulation

let private stagesForJob (job: Job) =
    match job.Id with
    | Barista -> Careers.BaristaCareer.stages
    | Bartender -> Careers.BartenderCareer.stages

let private fulfillsRequirements state nextStage =
    let character = Queries.Characters.playableCharacter state

    nextStage.Requirements
    |> List.forall (function
        | CareerStageRequirement.Skill(skillId, minLevel) ->
            let _, currentSkillLevel =
                Queries.Skills.characterSkillWithLevel
                    state
                    character.Id
                    skillId

            currentSkillLevel >= minLevel)

let private givePromotionChance (job: Job) nextStage =
    if RandomGen.chance 10 then
        let cityId, placeId = job.Location

        let nextStageWithAdjustedSalary =
            Common.createCareerStage cityId placeId nextStage

        let adjustedSalary = nextStageWithAdjustedSalary.BaseSalaryPerDayMoment

        [ CareerPromoted(
              { job with
                  CurrentStage = nextStageWithAdjustedSalary },
              adjustedSalary
          ) ]
    else
        []

/// Gives a 10% chance of promotion if the character fulfills the requirements
/// for the next stage of the job, if any.
let promoteIfNeeded job state =
    let (CareerStageId currentStageIdx) = job.CurrentStage.Id

    let nextStage =
        stagesForJob job |> List.tryItem (currentStageIdx + 1uy |> int)

    nextStage
    |> Option.bind (fun nextStage ->
        if fulfillsRequirements state nextStage then
            Some nextStage
        else
            None)
    |> Option.map (givePromotionChance job)
    |> Option.defaultValue
        [] (* Either no promotions available or chance didn't succeed. *)

module Duets.Simulation.Careers.Promotion

open Duets.Entities
open Duets.Data
open Duets.Simulation
open Duets.Simulation.Careers.Common

let private stagesForJob (job: Job) =
    match job.Id with
    | Barista -> Careers.BaristaCareer.stages
    | Bartender -> Careers.BartenderCareer.stages
    | Chef -> Careers.ChefCareer.stages
    | MusicProducer -> Careers.MusicProducerCareer.stages
    | RadioHost -> Careers.RadioHostCareer.stages

let private givePromotionChance (job: Job) nextStage =
    if RandomGen.chance 10 then
        let cityId, placeId, _ = job.Location

        let nextStageWithAdjustedSalary =
            createCareerStage cityId placeId nextStage

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
        let canBePromoted =
            fulfillsRequirements state nextStage.Requirements 0<adjustment>

        if canBePromoted then Some nextStage else None)
    |> Option.map (givePromotionChance job)
    |> Option.defaultValue
        [] (* Either no promotions available or chance didn't succeed. *)

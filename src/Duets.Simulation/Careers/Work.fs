module Duets.Simulation.Careers.Work

open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Bank.Operations
open Duets.Simulation.Character
open Duets.Simulation.Time

let private workAttributeChange state (job: Job) =
    let character = Queries.Characters.playableCharacter state

    job.ShiftAttributeEffect
    |> List.collect (fun attributeChange ->
        attributeChange ||> Attribute.add character)

/// Starts a work shift in the given job, passing the necessary amount of day
/// moments until the shift ends, paying the character the earned amount and
/// reducing the needed attributes from the character.
let workShift state job =
    let shiftDayMoments =
        CareerInteraction.Work job
        |> Interaction.Career
        |> Queries.InteractionTime.timeRequired

    let timeEffects = AdvanceTime.advanceDayMoment' state shiftDayMoments

    let characterAccount = Queries.Bank.playableCharacterAccount state

    let shiftSalary =
        job.CurrentStage.BaseSalaryPerDayMoment * decimal shiftDayMoments

    let shiftPay = shiftSalary |> income state characterAccount

    let attributeEffects = workAttributeChange state job

    [ yield CareerShiftPerformed(job, shiftSalary)
      yield! timeEffects
      yield shiftPay
      yield! attributeEffects ]

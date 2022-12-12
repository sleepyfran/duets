module Simulation.Careers.Work

open Entities
open Simulation
open Simulation.Bank.Operations
open Simulation.Character
open Simulation.Time

let workAttributeChange state (job: Job) =
    let character = Queries.Characters.playableCharacter state

    match job.Id with
    | Bartender ->
        [ Attribute.add character CharacterAttribute.Energy -20
          Attribute.add character CharacterAttribute.Mood -10
          Attribute.add character CharacterAttribute.Health -2 ]

/// Starts a work shift in the given job, passing the necessary amount of day
/// moments until the shift ends, paying the character the earned amount and
/// reducing the needed attributes from the character.
let workShift state (job: Job) =
    let shiftDayMoments =
        match job.Schedule with
        | JobSchedule.Free duration -> duration

    let timeEffects = AdvanceTime.advanceDayMoment' state shiftDayMoments

    let characterAccount = Queries.Bank.playableCharacterAccount state

    let shiftPay =
        job.CurrentStage.BaseSalaryPerDayMoment * decimal shiftDayMoments
        |> income state characterAccount

    let attributeEffects = workAttributeChange state job

    [ yield! timeEffects; yield shiftPay; yield! attributeEffects ]

module Duets.Simulation.Careers.Work

open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Bank.Operations
open Duets.Simulation.Character
open Duets.Simulation.Time

let private workAttributeChange state (job: Job) =
    let character = Queries.Characters.playableCharacter state
    let shiftDuration = Career.jobDuration job / 1<dayMoments>

    job.CurrentStage.ShiftAttributeEffect
    |> List.collect (fun (attribute, amount) ->
        (attribute, amount * shiftDuration) ||> Attribute.add character)

let private dayMomentsUntilClosingTime state job =
    let currentTime = Queries.Calendar.today state
    let jobPlace = job.Location ||> Queries.World.placeInCityById

    match jobPlace.OpeningHours with
    | PlaceOpeningHours.OpeningHours(_, dayMoments) ->
        let closedDayMoment =
            dayMoments |> List.last |> Calendar.Query.nextDayMoment

        let closingTime =
            currentTime |> Calendar.Transform.changeDayMoment closedDayMoment

        Calendar.Query.dayMomentsBetween currentTime closingTime |> Some
    | PlaceOpeningHours.AlwaysOpen -> None

let private timeAdvancement state job =
    let dayMomentsUntilClosing = dayMomentsUntilClosingTime state job
    let shiftDayMoments = Career.jobDuration job

    match dayMomentsUntilClosing with
    | Some dayMoments -> min shiftDayMoments dayMoments
    | None -> shiftDayMoments

/// Starts a work shift in the given job, passing the necessary amount of day
/// moments until the shift ends, paying the character the earned amount and
/// reducing the needed attributes from the character.
let workShift state job =
    let characterAccount = Queries.Bank.playableCharacterAccount state

    let shiftDayMoments = timeAdvancement state job

    // TODO: Migrate to common time advancement once we migrate to actions.
    let timeEffects = AdvanceTime.advanceDayMoment' state shiftDayMoments

    let shiftSalary =
        job.CurrentStage.BaseSalaryPerDayMoment * decimal shiftDayMoments

    let shiftPay = shiftSalary |> income state characterAccount

    let attributeEffects = workAttributeChange state job

    [ yield CareerShiftPerformed(job, shiftSalary)
      yield! timeEffects
      yield shiftPay
      yield! attributeEffects ]

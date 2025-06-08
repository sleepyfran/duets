module Duets.Simulation.Careers.Work

open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Bank.Operations
open Duets.Simulation.Character

let private workAttributeChange state (job: Job) =
    let character = Queries.Characters.playableCharacter state
    let shiftDuration = Career.jobDuration job / 1<dayMoments>

    job.CurrentStage.ShiftAttributeEffect
    |> List.collect (fun (attribute, amount) ->
        (attribute, amount * shiftDuration) ||> Attribute.add character)

let private dayMomentsUntilClosingTime state job =
    let currentTime = Queries.Calendar.today state
    let cityId, placeId, _ = job.Location
    let jobPlace = Queries.World.placeInCityById cityId placeId

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

type WorkshiftError = | AttemptedToWorkDuringClosingTime

/// Starts a work shift in the given job, passing the necessary amount of day
/// moments until the shift ends, paying the character the earned amount and
/// reducing the needed attributes from the character.
let rec workShift state job =
    let currentPlace = Queries.World.currentPlace state
    let currentTime = Queries.Calendar.today state

    let currentlyClosed =
        Queries.World.placeCurrentlyOpen currentPlace currentTime |> not

    if currentlyClosed then
        Error AttemptedToWorkDuringClosingTime
    else
        workShift' state job |> Ok

and private workShift' state job =
    let characterAccount = Queries.Bank.playableCharacterAccount state

    let shiftDayMoments = timeAdvancement state job

    let shiftSalary =
        job.CurrentStage.BaseSalaryPerDayMoment * decimal shiftDayMoments

    let shiftPay = shiftSalary |> income state characterAccount

    let attributeEffects = workAttributeChange state job

    [ yield CareerShiftPerformed(job, shiftDayMoments, shiftSalary)
      yield shiftPay
      yield! attributeEffects ]

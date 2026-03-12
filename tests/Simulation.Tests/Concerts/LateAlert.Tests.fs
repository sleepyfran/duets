module Duets.Simulation.Tests.Concerts.LateAlert

open NUnit.Framework
open FsUnit
open Test.Common

open Duets.Data.World
open Duets.Entities
open Duets.Simulation

let concertAtNight =
    { dummyConcert with
        DayMoment = Night }

let concertDate =
    concertAtNight.Date
    |> Calendar.Transform.changeDayMoment concertAtNight.DayMoment

let stateWithConcert =
    dummyState
    |> State.Concerts.addScheduledConcert
        dummyBand
        (ScheduledConcert(concertAtNight, dummyToday))

[<Test>]
let ``returns CharacterRunningLateToConcert when date matches and character is not at venue``
    ()
    =
    let effects =
        Concerts.LateAlert.checkIfRunningLate stateWithConcert concertDate

    effects |> should haveLength 1

    match effects with
    | [ CharacterRunningLateToConcert(_, concert) ] ->
        concert.Id |> should equal concertAtNight.Id
    | _ -> failwith "Expected CharacterRunningLateToConcert effect"

[<Test>]
let ``returns no effects when character is already at the concert venue`` () =
    let stateAtVenue =
        { stateWithConcert with
            CurrentPosition =
                (concertAtNight.CityId, concertAtNight.VenueId, Ids.Common.lobby) }

    let effects =
        Concerts.LateAlert.checkIfRunningLate stateAtVenue concertDate

    effects |> should be Empty

[<Test>]
let ``returns no effects when date does not match concert date`` () =
    let wrongDate =
        concertAtNight.Date
        |> Calendar.Ops.addDays 1<days>
        |> Calendar.Transform.changeDayMoment concertAtNight.DayMoment

    let effects =
        Concerts.LateAlert.checkIfRunningLate stateWithConcert wrongDate

    effects |> should be Empty

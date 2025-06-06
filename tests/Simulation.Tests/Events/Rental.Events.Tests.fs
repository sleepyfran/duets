module Duets.Simulation.Tests.Events.Rental

open FsCheck
open FsUnit
open NUnit.Framework
open Test.Common
open Test.Common.Generators

open Duets.Entities
open Duets.Simulation

let generateStateWithRental rental =
    State.generateOne State.defaultOptions |> State.Rentals.add rental

let createRental placeId rentalType =
    { Amount = 100m<dd>
      Coords = (dummyCity.Id, placeId)
      RentalType = rentalType }

let earlyMorningDate =
    dummyToday |> Calendar.Transform.changeDayMoment EarlyMorning

let morningDate = earlyMorningDate |> Calendar.Query.next
let dayBeforeYesterday = dummyToday |> Calendar.Ops.addDays -2<days>
let yesterday = dummyToday |> Calendar.Ops.addDays -1<days>
let nextSeason = dummyToday |> Calendar.Ops.addSeasons 1

let filterRentalExpired =
    List.filter (function
        | RentalExpired _ -> true
        | _ -> false)

let filterRentalKickedOut =
    List.filter (function
        | RentalKickedOut _ -> true
        | _ -> false)

[<Test>]
let ``tick of time passed in the morning should expire rental if it is due``
    ()
    =
    let state =
        Seasonal yesterday
        |> createRental dummyPlace.Id
        |> generateStateWithRental

    Simulation.tickOne state (TimeAdvanced morningDate)
    |> fst
    |> filterRentalExpired
    |> List.head
    |> should be (ofCase <@ RentalExpired @>)

[<Test>]
let ``tick of time passed in any other day moment should not expire rentals even if they are due``
    ()
    =
    [ EarlyMorning; Midday; Evening; Afternoon; Night; Midnight ]
    |> List.iter (fun dayMoment ->
        let state =
            Seasonal yesterday
            |> createRental dummyPlace.Id
            |> generateStateWithRental

        let date = dummyToday |> Calendar.Transform.changeDayMoment dayMoment

        Simulation.tickOne state (TimeAdvanced date)
        |> fst
        |> filterRentalExpired
        |> should haveLength 0)

[<Test>]
let ``rental expiring in the current location results in player getting kicked out``
    ()
    =
    let state =
        Seasonal yesterday
        |> createRental dummyPlace.Id
        |> generateStateWithRental
        |> State.World.move dummyCity.Id dummyPlace.Id 0

    Simulation.tickOne state (TimeAdvanced morningDate)
    |> fst
    |> filterRentalKickedOut
    |> List.head
    |> should be (ofCase <@ RentalKickedOut @>)

[<Test>]
let ``rental expiring outside of current location does not generate kicked out effect``
    ()
    =
    let state =
        Seasonal yesterday
        |> createRental dummyPlace.Id
        |> generateStateWithRental

    Simulation.tickOne state (TimeAdvanced morningDate)
    |> fst
    |> filterRentalKickedOut
    |> should haveLength 0

[<Test>]
let ``if the character has multiple rentals, one which has expired and another one that has not, the expired one should be raised as an effect``
    ()
    =
    let expiredRental =
        OneTime(dayBeforeYesterday, yesterday) |> createRental dummyHotel2.Id

    let notExpiredRental = Seasonal nextSeason |> createRental dummyHotel1.Id

    let state =
        notExpiredRental
        |> generateStateWithRental
        |> State.Rentals.add expiredRental

    Simulation.tickOne state (TimeAdvanced morningDate)
    |> fst
    |> filterRentalExpired
    |> List.head
    |> should be (ofCase <@ RentalExpired @>)

module Simulation.Tests.Notifications

open FsCheck
open FsUnit
open NUnit.Framework

open Entities
open Simulation.Calendar
open Test.Common
open Test.Common.Generators

let private createFlightGen fromDate toDate =
    gen {
        let! defaultFlight = Arb.generate<Flight>

        let! flightDate = Date.dateGenerator fromDate toDate

        return
            { defaultFlight with
                AlreadyUsed = false
                Date = flightDate
                DayMoment = Calendar.Query.dayMomentOf toDate }
    }

[<Test>]
let ``createHappeningSoon returns nothing when no events are scheduled`` () =
    let state = State.generateOne State.defaultOptions

    Notifications.createHappeningSoon state dummyToday |> should be Empty

[<Test>]
let ``createHappeningSoon returns nothing if the next event is happening after the next day moment``
    ()
    =
    let flightGen = createFlightGen dummyToday dummyTodayMiddleOfYear

    State.generateN
        { State.defaultOptions with
            FlightsToGenerate = 10
            FlightGen = flightGen }
        10
    |> List.iter (fun state ->
        Notifications.createHappeningSoon state dummyToday |> should be Empty)

[<Test>]
let ``createHappeningSoon returns flight event if it's happening in the day moment after the current one``
    ()
    =
    let nextDayMomentFromDummy = dummyToday |> Calendar.Query.next

    let flightGen =
        createFlightGen nextDayMomentFromDummy nextDayMomentFromDummy

    let state =
        State.generateOne
            { State.defaultOptions with
                FlightsToGenerate = 1
                FlightGen = flightGen }

    let effects = Notifications.createHappeningSoon state dummyToday
    let firstEffect = effects |> List.head

    match firstEffect with
    | NotificationEventHappeningSoon calendarEventType ->
        calendarEventType |> should be (ofCase <@ CalendarEventType.Flight @>)
    | _ -> failwith "Incorrect effect raised!"

[<Test>]
let ``createHappeningSoon returns concert event if it's happening in the day moment after the current one``
    ()
    =
    let nextDayMomentFromDummy = dummyToday |> Calendar.Query.next

    let concertGen =
        Concert.scheduledConcertGenerator
            { Concert.defaultOptions with
                From = nextDayMomentFromDummy
                To = nextDayMomentFromDummy
                DayMoment = Calendar.Query.dayMomentOf nextDayMomentFromDummy }

    let state =
        State.generateOne
            { State.defaultOptions with
                FutureConcertsToGenerate = 1
                ScheduledConcertGen = concertGen }

    let effects = Notifications.createHappeningSoon state dummyToday
    let firstEffect = effects |> List.head

    match firstEffect with
    | NotificationEventHappeningSoon calendarEventType ->
        calendarEventType |> should be (ofCase <@ CalendarEventType.Concert @>)
    | _ -> failwith "Incorrect effect raised!"

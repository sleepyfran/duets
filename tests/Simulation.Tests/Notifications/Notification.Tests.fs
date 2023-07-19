module Duets.Simulation.Tests.Notifications

open FsCheck
open FsUnit
open NUnit.Framework

open Duets.Entities
open Duets.Simulation.Calendar
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
let ``createNotifications returns nothing when no events are scheduled`` () =
    let state = State.generateOne State.defaultOptions

    Notifications.createNotifications state dummyToday |> should be Empty

[<Test>]
let ``createNotifications returns nothing if the next event is happening after two day moments``
    ()
    =
    let beginningDate = dummyToday |> Calendar.Query.next |> Calendar.Query.next

    let flightGen =
        beginningDate
        |> Calendar.Ops.addMonths 6
        |> createFlightGen beginningDate

    State.generateN
        { State.defaultOptions with
            FlightsToGenerate = 10
            FlightGen = flightGen }
        10
    |> List.iter (fun state ->
        Notifications.createNotifications state dummyToday |> should be Empty)

[<Test>]
let ``createNotifications returns flight event if it's happening in the day moment after the current one``
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

    let effects = Notifications.createNotifications state dummyToday
    let firstEffect = effects |> List.head

    match firstEffect with
    | Notification(Notification.CalendarEvent calendarEventType) ->
        calendarEventType |> should be (ofCase <@ CalendarEventType.Flight @>)
    | _ -> failwith "Incorrect effect raised!"

[<Test>]
let ``createNotifications returns concert event if it's happening in the day moment after the current one``
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

    let effects = Notifications.createNotifications state dummyToday
    let firstEffect = effects |> List.head

    match firstEffect with
    | Notification(Notification.CalendarEvent calendarEventType) ->
        calendarEventType |> should be (ofCase <@ CalendarEventType.Concert @>)
    | _ -> failwith "Incorrect effect raised!"

[<Test>]
let ``createNotifications returns rental payment reminder if a monthly rental will expire a week from the current date``
    ()
    =
    let nextWeekFromDummyToday = dummyToday |> Calendar.Ops.addDays 7

    let dummyRental =
        { RentalType = Monthly nextWeekFromDummyToday
          Amount = 900m<dd>
          Coords = dummyCity.Id, dummyPlace.Id }

    let state =
        State.generateOne
            { State.defaultOptions with
                FutureConcertsToGenerate = 0 }

    let state =
        { state with
            Rentals =
                [ (dummyCity.Id, dummyPlace.Id), dummyRental ] |> Map.ofList }

    let effects = Notifications.createNotifications state dummyToday
    let firstEffect = effects |> List.head

    match firstEffect with
    | Notification(Notification.RentalNotification rentalNotificationType) ->
        rentalNotificationType
        |> should be (ofCase <@ RentalNotificationType.RentalDueInOneWeek @>)
    | _ -> failwith "Incorrect effect raised!"

[<Test>]
let ``createNotifications returns rental payment reminder if a monthly rental will expire tomorrow``
    ()
    =
    let tomorrowFromDummyToday = dummyToday |> Calendar.Ops.addDays 1

    let dummyRental =
        { RentalType = Monthly tomorrowFromDummyToday
          Amount = 900m<dd>
          Coords = dummyCity.Id, dummyPlace.Id }

    let state =
        State.generateOne
            { State.defaultOptions with
                FutureConcertsToGenerate = 0 }

    let state =
        { state with
            Rentals =
                [ (dummyCity.Id, dummyPlace.Id), dummyRental ] |> Map.ofList }

    let effects = Notifications.createNotifications state dummyToday
    let firstEffect = effects |> List.head

    match firstEffect with
    | Notification(Notification.RentalNotification rentalNotificationType) ->
        rentalNotificationType
        |> should be (ofCase <@ RentalNotificationType.RentalDueTomorrow @>)
    | _ -> failwith "Incorrect effect raised!"

[<Test>]
let ``createNotifications returns nothing for rentals that are one time`` () =
    let nextWeekFromDummyToday = dummyToday |> Calendar.Ops.addDays 7

    let dummyRental =
        { RentalType = OneTime(dummyToday, nextWeekFromDummyToday)
          Amount = 900m<dd>
          Coords = dummyCity.Id, dummyPlace.Id }

    let state =
        State.generateOne
            { State.defaultOptions with
                FutureConcertsToGenerate = 0 }

    let state =
        { state with
            Rentals =
                [ (dummyCity.Id, dummyPlace.Id), dummyRental ] |> Map.ofList }

    let effects = Notifications.createNotifications state dummyToday
    effects |> should be Empty

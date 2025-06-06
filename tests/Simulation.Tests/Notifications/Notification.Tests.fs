module Duets.Simulation.Tests.Notifications

open FsCheck
open FsUnit
open NUnit.Framework

open Duets.Entities
open Duets.Simulation
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
let ``create returns an effect scheduling the notification`` () =
    let normalizedTomorrow =
        dummyToday
        |> Calendar.Ops.addDays 1<days>
        |> Calendar.Transform.resetDayMoment

    let tomorrow =
        normalizedTomorrow
        |> Calendar.Transform.changeDayMoment
            Night (* To test date normalization. *)

    let dummyRental =
        { RentalType = Seasonal tomorrow
          Amount = 900m<dd>
          Coords = dummyCity.Id, dummyPlace.Id }

    let dummyNotification =
        Notification.RentalNotification(
            RentalNotificationType.RentalDueTomorrow dummyRental
        )

    let effect = Notifications.create tomorrow Night dummyNotification

    match effect with
    | NotificationScheduled(date, dayMoment, notification) ->
        date |> should equal normalizedTomorrow
        dayMoment |> should equal Night
        notification |> should equal dummyNotification
    | _ -> failwith "Unexpected effect"

[<Test>]
let ``showPendingNotifications returns nothing when no events are scheduled``
    ()
    =
    let state = State.generateOne State.defaultOptions

    Notifications.showPendingNotifications state dummyToday |> should be Empty

[<Test>]
let ``showPendingNotifications returns notifications scheduled on the state``
    ()
    =
    let dummyRental =
        { RentalType = Seasonal dummyToday
          Amount = 900m<dd>
          Coords = dummyCity.Id, dummyPlace.Id }

    let dummyNotification =
        Notification.RentalNotification(
            RentalNotificationType.RentalDueTomorrow dummyRental
        )

    let effect = Notifications.create dummyToday Night dummyNotification

    let state = Simulation.tickOne dummyState effect |> snd

    let dateOfSchedule = dummyToday |> Calendar.Transform.changeDayMoment Night

    let effects = Notifications.showPendingNotifications state dateOfSchedule
    let firstEffect = effects |> List.head

    match firstEffect with
    | NotificationShown(Notification.RentalNotification rentalNotificationType) ->
        rentalNotificationType
        |> should be (ofCase <@ RentalNotificationType.RentalDueTomorrow @>)
    | _ -> failwith "Incorrect effect raised!"


[<Test>]
let ``showPendingNotifications returns nothing if the next event is happening after two day moments``
    ()
    =
    let beginningDate = dummyToday |> Calendar.Query.next |> Calendar.Query.next

    let flightGen =
        beginningDate
        |> Calendar.Ops.addSeasons 6
        |> createFlightGen beginningDate

    State.generateN
        { State.defaultOptions with
            FlightsToGenerate = 10
            FlightGen = flightGen }
        10
    |> List.iter (fun state ->
        Notifications.showPendingNotifications state dummyToday
        |> should be Empty)

[<Test>]
let ``showPendingNotifications returns flight event if it's happening in 4 day moments``
    ()
    =
    let in4DayMoments = dummyToday |> Calendar.Query.nextN 4

    let flightGen = createFlightGen in4DayMoments in4DayMoments

    let state =
        State.generateOne
            { State.defaultOptions with
                FlightsToGenerate = 1
                FlightGen = flightGen }

    let effects = Notifications.showPendingNotifications state dummyToday
    let firstEffect = effects |> List.head

    match firstEffect with
    | NotificationShown(Notification.CalendarEvent calendarEventType) ->
        calendarEventType |> should be (ofCase <@ CalendarEventType.Flight @>)
    | _ -> failwith "Incorrect effect raised!"

[<Test>]
let ``showPendingNotifications returns concert event if it's happening in 4 day moments``
    ()
    =
    let in4DayMoments = dummyToday |> Calendar.Query.nextN 4

    let concertGen =
        Concert.scheduledConcertGenerator
            { Concert.defaultOptions with
                From = in4DayMoments
                To = in4DayMoments
                DayMoment = Calendar.Query.dayMomentOf in4DayMoments }

    let state =
        State.generateOne
            { State.defaultOptions with
                FutureConcertsToGenerate = 1
                ScheduledConcertGen = concertGen }

    let effects = Notifications.showPendingNotifications state dummyToday
    let firstEffect = effects |> List.head

    match firstEffect with
    | NotificationShown(Notification.CalendarEvent calendarEventType) ->
        calendarEventType |> should be (ofCase <@ CalendarEventType.Concert @>)
    | _ -> failwith "Incorrect effect raised!"

[<Test>]
let ``showPendingNotifications returns rental payment reminder if a seasonal rental will expire a week from the current date``
    ()
    =
    let nextWeekFromDummyToday = dummyToday |> Calendar.Ops.addDays 7<days>

    let dummyRental =
        { RentalType = Seasonal nextWeekFromDummyToday
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

    let effects = Notifications.showPendingNotifications state dummyToday
    let firstEffect = effects |> List.head

    match firstEffect with
    | NotificationShown(Notification.RentalNotification rentalNotificationType) ->
        rentalNotificationType
        |> should be (ofCase <@ RentalNotificationType.RentalDueInOneWeek @>)
    | _ -> failwith "Incorrect effect raised!"

[<Test>]
let ``showPendingNotifications returns rental payment reminder if a seasonal rental will expire tomorrow``
    ()
    =
    let tomorrowFromDummyToday = dummyToday |> Calendar.Ops.addDays 1<days>

    let dummyRental =
        { RentalType = Seasonal tomorrowFromDummyToday
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

    let effects = Notifications.showPendingNotifications state dummyToday
    let firstEffect = effects |> List.head

    match firstEffect with
    | NotificationShown(Notification.RentalNotification rentalNotificationType) ->
        rentalNotificationType
        |> should be (ofCase <@ RentalNotificationType.RentalDueTomorrow @>)
    | _ -> failwith "Incorrect effect raised!"

[<Test>]
let ``showPendingNotifications returns nothing for rentals that are one time``
    ()
    =
    let nextWeekFromDummyToday = dummyToday |> Calendar.Ops.addDays 7<days>

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

    let effects = Notifications.showPendingNotifications state dummyToday
    effects |> should be Empty

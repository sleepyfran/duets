module Duets.Simulation.Calendar.Notifications

open Duets.Entities
open Duets.Simulation

/// Checks if there's any pending notification to be sent about events that are
/// set to happen soon.
let private createHappeningSoon state date =
    let nextDayMomentDate = Calendar.Query.next date

    Queries.CalendarEvents.allOfDateMonth state date
    |> List.map snd
    |> List.concat
    |> List.choose (fun event ->
        let eventDate =
            CalendarEvent.date event ||> Calendar.Transform.changeDayMoment'

        if nextDayMomentDate = eventDate then
            Notification.CalendarEvent event |> Notification |> Some
        else
            None)

/// Checks if there's any pending notification to be sent about upcoming payments
/// in rentals.
let private createRentalDueNotifications state date =
    let tomorrowDate = date |> Calendar.Ops.addDays 1
    let nextWeekDate = date |> Calendar.Ops.addDays 7

    Queries.Rentals.allUpcoming state
    |> List.choose (fun rental ->
        match rental.RentalType with
        | Monthly nextPaymentDate ->
            match nextPaymentDate with
            | date when date = tomorrowDate ->
                RentalNotificationType.RentalDueTomorrow rental
                |> Notification.RentalNotification
                |> Notification
                |> Some
            | date when date = nextWeekDate ->
                RentalNotificationType.RentalDueInOneWeek rental
                |> Notification.RentalNotification
                |> Notification
                |> Some
            | _ -> None
        (* One time rentals should not have reminders. *)
        | _ -> None)

/// Creates all effects needed to be raised about notifications.
let createNotifications state date =
    createHappeningSoon state date @ createRentalDueNotifications state date

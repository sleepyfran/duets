module Duets.Simulation.Notifications

open Duets.Entities
open Duets.Simulation

/// Checks if there's any pending notification to be sent about events that are
/// set to happen soon.
let private createHappeningSoon state date =
    let dateInFiveDayMoments = date |> Calendar.Query.nextN 4

    Queries.CalendarEvents.allOfDateMonth state date
    |> List.map snd
    |> List.concat
    |> List.choose (fun event ->
        let eventDate =
            CalendarEvent.date event ||> Calendar.Transform.changeDayMoment'

        (* Notify the player if the event is happening in 5 day moments or right now. *)
        if dateInFiveDayMoments = eventDate || date = eventDate then
            Notification.CalendarEvent event |> Some
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
                |> Some
            | date when date = nextWeekDate ->
                RentalNotificationType.RentalDueInOneWeek rental
                |> Notification.RentalNotification
                |> Some
            | _ -> None
        (* One time rentals should not have reminders. *)
        | _ -> None)

/// Creates a notification to be shown on the given date and day moment. Takes
/// care of normalizing the date so that the time does not matter.
let create date dayMoment notification =
    let date = date |> Calendar.Transform.resetDayMoment
    NotificationScheduled(date, dayMoment, notification)

/// Creates all effects needed to be raised about notifications.
let showPendingNotifications state date =
    // TODO: Move to new notification system.
    let deprecated_notifications =
        createHappeningSoon state date @ createRentalDueNotifications state date

    let pendingNotifications = Queries.Notifications.forDate state date

    pendingNotifications @ deprecated_notifications
    |> List.map NotificationShown

module Duets.Simulation.Calendar.Notifications

open Duets.Entities
open Duets.Simulation

/// Checks if there's any pending notification to be sent and, if so, creates
/// the effects needed to notify the player about something that is going to
/// happen soon.
let createHappeningSoon state date =
    let nextDayMomentDate = Calendar.Query.next date

    Queries.CalendarEvents.allOfDateMonth state date
    |> List.map snd
    |> List.concat
    |> List.choose (fun event ->
        let eventDate =
            CalendarEvent.date event ||> Calendar.Transform.changeDayMoment'

        if nextDayMomentDate = eventDate then
            NotificationEventHappeningSoon event |> Some
        else
            None)

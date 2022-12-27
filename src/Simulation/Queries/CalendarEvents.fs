namespace Simulation.Queries

open Entities

module CalendarEvents =
    /// Returns all the concerts and flights scheduled in the month from the
    /// given date.
    let allOfDateMonth state date =
        let currentBand = Bands.currentBand state

        let flights =
            Flights.forMonth state date |> List.map CalendarEventType.Flight

        let concerts =
            Concerts.scheduleForMonth state currentBand.Id date
            |> Seq.map (Concert.fromScheduled >> CalendarEventType.Concert)
            |> List.ofSeq

        (* Put them together and group by the date without taking the hour into consideration. *)
        flights @ concerts
        |> List.sortBy CalendarEvent.date
        |> List.groupBy CalendarEvent.date

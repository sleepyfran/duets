namespace Duets.Simulation.Queries

open Duets.Entities

module CalendarEvents =
    /// Returns all the concerts and flights scheduled in the month from the
    /// given date.
    let allOfDateSeason state date =
        let currentBand = Bands.currentBand state

        let flights =
            Flights.forMonth state date |> List.map CalendarEventType.Flight

        let concerts =
            Concerts.scheduleForSeason state currentBand.Id date
            |> Seq.map (Concert.fromScheduled >> CalendarEventType.Concert)
            |> List.ofSeq

        (* Put them together and group by the date without taking the hour into consideration. *)
        flights @ concerts
        |> List.sortBy CalendarEvent.date
        |> List.groupBy CalendarEvent.date

    /// Returns all flights and concerts scheduled in the given date.
    let forDayMoment state date dayMoment =
        let currentBand = Bands.currentBand state

        let flights =
            Flights.forDay state date
            |> List.filter (fun flight -> flight.DayMoment = dayMoment)
            |> List.map CalendarEventType.Flight

        let concerts =
            Concerts.scheduleForDay state currentBand.Id date
            |> Option.bind (fun scheduledConcert ->
                let concert = Concert.fromScheduled scheduledConcert
                if concert.DayMoment = dayMoment then Some concert else None)
            |> Option.map CalendarEventType.Concert
            |> Option.toList

        flights @ concerts
        |> List.sortBy CalendarEvent.date
        |> List.groupBy CalendarEvent.date

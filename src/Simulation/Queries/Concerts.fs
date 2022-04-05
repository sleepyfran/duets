module Simulation.Queries.Concerts

open Aether
open Aether.Operators
open Entities
open Simulation

let private bandSchedule state bandId =
    let concertsLens =
        Lenses.FromState.Concerts.allByBand_ bandId

    Optic.get concertsLens state
    |> Option.defaultValue Concert.Timeline.empty

/// Returns a concert, if any scheduled, for the given band and date.
let scheduleForDay state bandId date =
    let timeline = bandSchedule state bandId

    timeline.ScheduledEvents
    |> Seq.tryFind
        (fun event ->
            Concert.fromScheduled event
            |> fun concert ->
                let concertDate =
                    concert.Date |> Calendar.Transform.resetDayMoment

                let date =
                    date |> Calendar.Transform.resetDayMoment

                concertDate = date)

/// Returns a concert, if any scheduled, for the given band today.
let scheduleForTodayInPlace state bandId placeId =
    Queries.Calendar.today state
    |> scheduleForDay state bandId
    |> Option.bind
        (fun scheduledConcert ->
            let concert = Concert.fromScheduled scheduledConcert

            if concert.VenueId = placeId then
                Some scheduledConcert
            else
                None)

/// Returns the concerts that are scheduled around today. Adds any concert that
/// is currently scheduled today or tomorrow or happened in the last day.
let scheduledAroundDate state bandId =
    let today = Queries.Calendar.today state
    let timeline = bandSchedule state bandId

    let nextScheduledConcert =
        Seq.tryHead timeline.ScheduledEvents
        |> Option.map Concert.fromScheduled
        |> Option.bind
            (fun concert ->
                let spanBetween = concert.Date - today

                if spanBetween.Days <= 1 then
                    Some concert
                else
                    None)

    let lastPerformedConcert =
        Seq.tryLast timeline.PastEvents
        |> Option.map Concert.fromPast
        |> Option.bind
            (fun concert ->
                let spanBetween = today - concert.Date

                if spanBetween.Days <= 1 then
                    Some concert
                else
                    None)

    [ nextScheduledConcert
      lastPerformedConcert ]
    |> List.choose id

/// Returns all date from today to the end of the month that have a concert
/// scheduled.
let scheduleForMonth state bandId fromDay =
    Calendar.Query.monthDaysFrom fromDay
    |> Seq.choose (scheduleForDay state bandId)

/// Returns all scheduled concerts.
let allScheduled state bandId =
    let lenses =
        Lenses.FromState.Concerts.allByBand_ bandId
        >?> Lenses.Concerts.Timeline.scheduled_

    Optic.get lenses state
    |> Option.defaultValue Set.empty

/// Returns all past concerts.
let allPast state bandId =
    let lenses =
        Lenses.FromState.Concerts.allByBand_ bandId
        >?> Lenses.Concerts.Timeline.pastEvents_

    Optic.get lenses state
    |> Option.defaultValue Set.empty

/// Returns the last concert that happened in the city, if any.
let lastConcertInCity state bandId cityId =
    let lenses =
        Lenses.FromState.Concerts.allByBand_ bandId
        >?> Lenses.Concerts.Timeline.pastEvents_

    Optic.get lenses state
    |> Option.defaultValue Set.empty
    |> Set.toSeq
    |> Seq.filter
        (fun concert ->
            Concert.fromPast concert
            |> fun c -> c.CityId = cityId)
    |> Seq.sortByDescending
        (fun concert -> Concert.fromPast concert |> fun c -> c.Date)
    |> Seq.tryHead

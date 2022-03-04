module Simulation.Queries.Concerts

open Aether
open Aether.Operators
open Entities

/// Retrieves the complete information of a concert, which basically resolves
/// the ID that are given inside of the `CityId` and `VenueId` fields.
let info state concert =
    let concertCity =
        World.cityById state concert.CityId |> Option.get

    let concertVenue =
        World.concertSpaceById state concert.CityId concert.VenueId
        |> Option.get

    {| Id = concert.Id
       Date = concert.Date
       DayMoment = concert.DayMoment
       City = concertCity
       Venue = concertVenue
       TicketPrice = concert.TicketPrice
       TicketsSold = concert.TicketsSold |}

/// Returns a concert, if any scheduled, for the given band and date.
let scheduleForDay state bandId date =
    let concertsLens =
        Lenses.FromState.Concerts.allByBand_ bandId

    Optic.get concertsLens state
    |> Option.defaultValue Concert.Timeline.empty
    |> fun timeline ->
        timeline.PastEvents
        |> Seq.tryFind
            (fun event ->
                match event with
                | PerformedConcert (concert, _) -> concert.Date
                | FailedConcert concert -> concert.Date
                |> (=) date)

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

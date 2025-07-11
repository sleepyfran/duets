module Duets.Simulation.Queries.Concerts

open Aether
open Aether.Operators
open Duets.Common
open Duets.Entities
open Duets.Simulation

let private bandSchedule state bandId =
    let concertsLens = Lenses.FromState.Concerts.allByBand_ bandId

    Optic.get concertsLens state |> Option.defaultValue Concert.Timeline.empty

/// Returns a concert, if any scheduled, for the given band and date.
let scheduleForDay state bandId date =
    let timeline = bandSchedule state bandId

    timeline.ScheduledEvents
    |> Seq.tryFind (fun event ->
        Concert.fromScheduled event
        |> fun concert ->
            let concertDate = concert.Date |> Calendar.Transform.resetDayMoment

            let date = date |> Calendar.Transform.resetDayMoment

            concertDate = date)

/// Returns a concert, if any scheduled, for the given band today.
let scheduleForTodayInPlace state bandId placeId =
    Queries.Calendar.today state
    |> scheduleForDay state bandId
    |> Option.bind (fun scheduledConcert ->
        let concert = Concert.fromScheduled scheduledConcert

        if concert.VenueId = placeId then
            Some scheduledConcert
        else
            None)

/// Returns a concert, if any scheduled, for the current day moment and date.
let scheduledForRightNow state bandId placeId =
    let currentDayMoment =
        Queries.Calendar.today state |> Calendar.Query.dayMomentOf

    scheduleForTodayInPlace state bandId placeId
    |> Option.bind (fun scheduledConcert ->
        let concert = Concert.fromScheduled scheduledConcert

        if concert.DayMoment = currentDayMoment then
            Some scheduledConcert
        else
            None)

/// Returns the concerts that are scheduled around today. Adds any concert that
/// is currently scheduled today or tomorrow or happened in the last day.
let scheduledAroundDate state bandId =
    let today = Queries.Calendar.today state
    let timeline = bandSchedule state bandId

    let aroundCurrentDate concert =
        let daysBetween = Calendar.Query.daysBetween concert.Date today
        if daysBetween <= 1<days> then Some concert else None

    let concertsScheduledAroundCurrentDate =
        timeline.ScheduledEvents
        |> Seq.choose (fun event ->
            Concert.fromScheduled event |> aroundCurrentDate)

    let lastPerformedAroundCurrentDate =
        timeline.PastEvents
        |> Seq.choose (fun event -> Concert.fromPast event |> aroundCurrentDate)

    [ yield! concertsScheduledAroundCurrentDate
      yield! lastPerformedAroundCurrentDate ]

/// Returns all date from today to the end of the season that have a concert
/// scheduled.
let scheduleForSeason state bandId fromDay =
    Calendar.Query.seasonDaysFrom fromDay
    |> Seq.choose (scheduleForDay state bandId)

/// Returns all scheduled concerts.
let allScheduled state bandId =
    let lenses =
        Lenses.FromState.Concerts.allByBand_ bandId
        >?> Lenses.Concerts.Timeline.scheduled_

    Optic.get lenses state |> Option.defaultValue List.empty

/// Returns all past concerts.
let allPast state bandId =
    let lenses =
        Lenses.FromState.Concerts.allByBand_ bandId
        >?> Lenses.Concerts.Timeline.pastEvents_

    Optic.get lenses state |> Option.defaultValue List.empty

/// Returns the last concert that happened in the city, if any.
let lastConcertInCity state bandId cityId =
    let lenses =
        Lenses.FromState.Concerts.allByBand_ bandId
        >?> Lenses.Concerts.Timeline.pastEvents_

    Optic.get lenses state
    |> Option.defaultValue List.empty
    |> List.filter (fun concert ->
        Concert.fromPast concert |> fun c -> c.CityId = cityId)
    |> List.tryHead

/// Calculates the percentage of people that came to the concert out of the
/// entire capacity of the venue.
let attendancePercentage concert =
    let venue = Queries.World.placeInCityById concert.CityId concert.VenueId

    let capacity =
        match venue.PlaceType with
        | PlaceType.ConcertSpace concertSpace -> concertSpace.Capacity
        | _ ->
            (* We don't really support concerts outside of a concert space, but
            lets just returned this to make it "compatible". *)
            0

    (float concert.TicketsSold / float capacity)
    |> (*) 100.0
    |> Math.roundToNearest

/// Calculates a fair ticket price for the concert, based on the fame of the
/// given band.
let fairTicketPrice state bandId =
    let bandFame = Bands.estimatedFameLevel state bandId

    match bandFame with
    | fame when fame < 25 -> 10.0m
    | fame when fame < 60 -> 25.0m
    | fame when fame < 80 -> 75.0m
    | _ -> 100.0m

/// Returns the range of capacity that the venue needs to have to be suitable
/// for the given band.
let suitableVenueCapacity state bandId cityId =
    let band = Bands.byId state bandId
    let fansInCity = Bands.fansInCity' band cityId

    match fansInCity with
    | fans when fans <= 1000<fans> -> (0, 300)
    | fans when fans <= 5000<fans> -> (0, 500)
    | fans when fans <= 20000<fans> -> (500, 5000)
    | fans when fans <= 100000<fans> -> (500, 20000)
    | _ -> (500, 500000)

/// Calculates the percentage off the tickets that the concert space will take
/// based on the quality of the place and the capacity.
let concertSpaceTicketPercentage (place: Place) =
    let capacity =
        match place.PlaceType with
        | PlaceType.ConcertSpace concertSpace -> concertSpace.Capacity
        | _ -> 0

    let capacityFactor =
        match capacity with
        | capacity when capacity < 200 -> 0.35
        | capacity when capacity < 500 -> 0.5
        | capacity when capacity < 1000 -> 0.75
        | capacity when capacity < 10000 -> 0.85
        | _ -> 1.0

    let qualityFactor = place.Quality / 1<quality> |> float

    ((qualityFactor / 100.0) * capacityFactor * 40.0) / 100.0

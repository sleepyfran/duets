module Duets.Simulation.Concerts.Scheduler

open Duets.Common
open Duets.Entities
open Duets.Simulation

type ScheduleError = | DateAlreadyScheduled

/// Validates that there's no other concert scheduled for the given date.
let validateNoOtherConcertsInDate state date =
    let currentBand = Queries.Bands.currentBand state

    let concertForDay =
        Queries.Concerts.scheduleForDay state currentBand.Id date

    if Option.isSome concertForDay then
        Error DateAlreadyScheduled
    else
        Ok date

/// Schedules a concert for the given date and day moment in the specified city
/// and venue for the current band.
let scheduleHeadlinerConcert state date dayMoment cityId venueId ticketPrice =
    let today = Queries.Calendar.today state

    let currentBand = Queries.Bands.currentBand state

    let concert =
        Concert.create date dayMoment cityId venueId ticketPrice Headliner

    ConcertScheduled(currentBand, ScheduledConcert(concert, today))

/// Fails the given concert with the given fail reason, creating a ConcertCancelled
/// effect to do so.
let failConcert state concert failReason =
    let band = Queries.Bands.currentBand state

    FailedConcert(concert, failReason) |> Tuple.two band |> ConcertCancelled

/// Checks whether there's any concert that was supposed to happen but didn't
/// (eg: the band didn't make it to the concert and therefore the concert
/// didn't happen) this creates the ConcertFailed effect which moves the concerts
/// to past concerts with the failed type.
let moveFailedConcerts state date =
    let currentBand = Queries.Bands.currentBand state

    let scheduledConcerts = Queries.Concerts.allScheduled state currentBand.Id

    scheduledConcerts
    |> Seq.choose (fun (ScheduledConcert(concert, _)) ->
        // The date by default does not include the day moment of the
        // concert, so we need to take it into account.
        let concertDate =
            concert.Date
            |> Calendar.Transform.changeDayMoment concert.DayMoment

        if date > concertDate then
            failConcert state concert BandDidNotMakeIt |> Some
        else
            None)
    |> List.ofSeq

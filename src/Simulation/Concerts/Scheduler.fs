module Simulation.Concerts.Scheduler

open Common
open Entities
open Simulation

type ScheduleError = | DateAlreadyScheduled

/// Validates that there's no other concert scheduled for the given date.
let validateNoOtherConcertsInDate state date =
    let currentBand =
        Queries.Bands.currentBand state

    let concertForDay =
        Queries.Concerts.scheduleForDay state currentBand.Id date

    if Option.isSome concertForDay then
        Error DateAlreadyScheduled
    else
        Ok date

/// Schedules a concert for the given date and day moment in the specified city
/// and venue for the current band.
let scheduleConcert state date dayMoment cityId venueId ticketPrice =
    let currentBand =
        Queries.Bands.currentBand state

    let concert =
        Concert.create date dayMoment cityId venueId ticketPrice

    ConcertScheduled(currentBand, (ScheduledConcert concert))

/// Checks whether there's any concert that was supposed to happen but didn't
/// (eg: the band didn't make it to the concert and therefore the concert
/// didn't happen) this creates the ConcertFailed effect which moves the concerts
/// to past concerts with the failed type.
let moveFailedConcerts state date =
    let currentBand =
        Queries.Bands.currentBand state

    let scheduledConcerts =
        Queries.Concerts.allScheduled state currentBand.Id

    scheduledConcerts
    |> Seq.choose (fun (ScheduledConcert concert) ->
        // The date by default does not include the day moment of the
        // concert, so we need to take it into account.
        let concertDate =
            concert.Date
            |> Calendar.Transform.changeDayMoment concert.DayMoment

        if date > concertDate then
            FailedConcert concert
            |> Tuple.two currentBand
            |> ConcertCancelled
            |> Some
        else
            None)
    |> List.ofSeq

let startScheduledConcerts state placeId =
    let situation =
        Queries.Situations.current state

    match situation with
    | Situation.InConcert _ ->
        [] (* Concert already started, no need to do anything. *)
    | _ ->
        let band = Queries.Bands.currentBand state

        (* Check whether we have a concert scheduled and, if so, initialize a new OngoingConcert. *)
        Queries.Concerts.scheduleForTodayInPlace state band.Id placeId
        |> Option.map (fun scheduledConcert ->
            let concert =
                Concert.fromScheduled scheduledConcert

            [ Situations.inConcert
                  { Events = []
                    Points = 0<quality>
                    Concert = concert } ])
        |> Option.defaultValue []

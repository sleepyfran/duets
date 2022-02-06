module Simulation.Scheduler

open Common
open Entities

type ScheduleError<'e> =
    | DateAlreadyScheduled
    | CreationError of 'e

/// Attempts to create a new concert from the parameters validating that is
/// valid and schedules it for the given date. Fails if the concert is not
/// correct (invalid ticket price, etc) or if the date is not available (already
/// scheduled).
let scheduleConcert state date dayMoment city venue ticketPrice =
    let currentBand = Queries.Bands.currentBand state

    let concertForDay =
        Queries.Schedule.concertsForDay state currentBand.Id date

    if Option.isSome concertForDay then
        Error DateAlreadyScheduled
    else
        ConcertContext.createConcert date dayMoment city venue ticketPrice
        |> Result.mapError CreationError
        |> Result.map (Tuple.two currentBand)
        |> Result.map ConcertScheduled

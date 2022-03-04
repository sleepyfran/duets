module Simulation.Concerts.Scheduler

open Entities
open Simulation

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
let scheduleConcert state date dayMoment cityId venueId ticketPrice =
    let currentBand = Queries.Bands.currentBand state

    let concert =
        Concert.create date dayMoment cityId venueId ticketPrice

    ConcertScheduled(currentBand, (ScheduledConcert concert))

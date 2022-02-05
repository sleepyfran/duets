module Simulation.Scheduler

open Common
open Entities

let scheduleConcert state date dayMoment city venue ticketPrice =
    let currentBand = Queries.Bands.currentBand state

    // TODO: Check for already scheduled concerts.
    ConcertContext.createConcert date dayMoment city venue ticketPrice
    |> Result.map (Tuple.two currentBand)
    |> Result.map ConcertScheduled

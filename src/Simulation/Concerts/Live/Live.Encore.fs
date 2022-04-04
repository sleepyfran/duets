module Simulation.Concerts.Live.Encore

open Entities

/// Adds a new encore to the list of events of the ongoing concert.
let doEncore ongoingConcert =
    response ongoingConcert (CommonEvent PerformedEncore) 0 ()

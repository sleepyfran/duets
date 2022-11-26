module Simulation.Events.Place

open Entities
open Simulation

/// Runs all the effects that are associated with the current place. This currently
/// just starts a scheduled concert when the character steps in the concert's
/// place, but it can be extended to do much more.
let internal run state =
    let currentPlace = Queries.World.currentPlace state

    match currentPlace.Type with
    | ConcertSpace _ ->
        Concerts.Scheduler.startScheduledConcerts state currentPlace.Id
    | _ -> []

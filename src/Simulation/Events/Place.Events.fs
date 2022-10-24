module Simulation.Events.Place

open Entities
open Simulation

/// Runs all the effects that are associated with the current place. This currently
/// just starts a scheduled concert when the character steps in the concert's
/// place, but it can be extended to do much more.
let internal run (_, placeId) state =
    let currentPlace =
        Queries.World.placeInCurrentCityById state placeId

    match currentPlace.Type with
    | ConcertSpace _ -> Concerts.Scheduler.startScheduledConcerts state placeId
    | _ -> []

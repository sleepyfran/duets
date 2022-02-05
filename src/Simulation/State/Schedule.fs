module Simulation.State.Schedule

open Aether
open Entities

let addConcert (concert: Concert) =
    let scheduledLens =
        Lenses.FromState.ScheduledEvents.date_ concert.Date

    let add = List.append [ (Concert concert.Id) ]

    Optic.map scheduledLens add

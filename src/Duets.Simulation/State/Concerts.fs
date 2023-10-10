module Duets.Simulation.State.Concerts

open Aether
open Aether.Operators
open Duets.Entities

let addScheduledConcert (band: Band) (concert: ScheduledConcert) =
    let concertsLens =
        Lenses.FromState.Concerts.allByBand_ band.Id
        >?> Lenses.Concerts.Timeline.scheduled_

    Optic.map concertsLens (Concert.Timeline.addScheduled concert)

let addPastConcert (band: Band) (concert: PastConcert) =
    let concertsLens =
        Lenses.FromState.Concerts.allByBand_ band.Id
        >?> Lenses.Concerts.Timeline.pastEvents_

    Optic.map concertsLens (Concert.Timeline.addPast concert)

let removeScheduledConcert (band: Band) (concert: Concert) =
    let concertsLens =
        Lenses.FromState.Concerts.allByBand_ band.Id
        >?> Lenses.Concerts.Timeline.scheduled_

    Optic.map
        concertsLens
        (List.filter (fun (ScheduledConcert(c, _)) -> c.Id <> concert.Id))

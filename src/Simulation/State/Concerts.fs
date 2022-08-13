module Simulation.State.Concerts

open Aether
open Aether.Operators
open Entities

let addScheduledConcert (band: Band) (concert: ScheduledConcert) =
    let concertsLens =
        Lenses.FromState.Concerts.allByBand_ band.Id
        >?> Lenses.Concerts.Timeline.scheduled_

    Optic.map concertsLens (Set.add concert)

let addPastConcert (band: Band) (concert: PastConcert) =
    let concertsLens =
        Lenses.FromState.Concerts.allByBand_ band.Id
        >?> Lenses.Concerts.Timeline.pastEvents_

    Optic.map concertsLens (Set.add concert)

let removeScheduledConcert (band: Band) (concert: Concert) =
    let concertsLens =
        Lenses.FromState.Concerts.allByBand_ band.Id
        >?> Lenses.Concerts.Timeline.scheduled_

    Optic.map
        concertsLens
        (Set.filter (fun (ScheduledConcert (c, _)) -> c.Id <> concert.Id))

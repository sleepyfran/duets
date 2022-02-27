module Simulation.State.Concerts

open Aether
open Aether.Operators
open Entities

let addConcert (band: Band) (concert: Concert) =
    let concertsLens =
        Lenses.FromState.Concerts.allByBand_ band.Id
        >?> Lenses.Concerts.Timeline.future_

    Optic.map concertsLens (Set.add concert)

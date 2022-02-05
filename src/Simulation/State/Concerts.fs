module Simulation.State.Concerts

open Aether
open Entities

let addConcert (band: Band) (concert: Concert) state =
    let concertsLens =
        Lenses.FromState.Concerts.allByBand_ band.Id

    let add = Map.add concert.Id concert

    Optic.map concertsLens add state

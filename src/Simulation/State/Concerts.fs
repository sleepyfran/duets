module Simulation.State.Concerts

open Aether
open Entities

let addConcert (band: Band) (concert: Concert) state =
    let concertsLens =
        Lenses.FromState.Concerts.allByBand_ band.Id

    Optic.map
        concertsLens
        (Map.change
            concert.Date
            (fun dayMomentMap ->
                dayMomentMap
                |> Option.defaultValue Map.empty
                |> Map.add concert.DayMoment concert
                |> Some))
        state

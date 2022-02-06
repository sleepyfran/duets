module Simulation.Queries.Schedule

open Aether
open Common
open Entities

/// Returns all concerts scheduled for the given date and band.
let concertsForDay state bandId date =
    let concertsLens =
        Lenses.FromState.Concerts.allByBand_ bandId

    Optic.get concertsLens state
    |> Option.defaultValue Calendar.Schedule.empty
    |> Map.tryFind date
    |> Option.defaultValue Calendar.Schedule.empty
    |> Map.tryHead

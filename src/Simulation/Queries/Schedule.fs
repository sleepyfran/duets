module Simulation.Queries.Schedule

open Aether
open Common
open Entities

/// Returns all concerts scheduled for the given date and band.
let concertForDay state bandId date =
    let concertsLens = Lenses.FromState.Concerts.allByBand_ bandId

    Optic.get concertsLens state
    |> Option.defaultValue Calendar.Schedule.empty
    |> Map.tryFind date
    |> Option.defaultValue Calendar.Schedule.empty
    |> Map.tryHead

/// Returns all date from today to the end of the month that have a concert
/// scheduled.
let concertScheduleForMonth state bandId fromDay =
    Calendar.Query.monthDaysFrom fromDay
    |> Seq.choose (concertForDay state bandId)

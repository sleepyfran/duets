module Simulation.Queries.Schedule

open Aether
open Common
open Entities

/// Returns all concerts scheduled for the given date and band.
let concertForDay state bandId date =
    let concertsLens =
        Lenses.FromState.Concerts.allByBand_ bandId

    Optic.get concertsLens state
    |> Option.defaultValue Calendar.Schedule.empty
    |> Map.tryFind date
    |> Option.defaultValue Calendar.Schedule.empty
    |> Map.tryHead

/// Returns all dates from today until the end of the month and Some concert
/// if there's one scheduled for that day or None if there's none in that day.
let concertScheduleForMonth state bandId fromDay =
    Calendar.Query.monthDaysFrom fromDay
    |> Seq.map
        (fun date ->
            let concert = concertForDay state bandId date
            date, concert)

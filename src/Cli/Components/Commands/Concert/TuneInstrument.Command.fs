namespace Cli.Components.Commands

open Cli.Components
open Cli.Components.Commands
open Cli.Text
open Simulation
open Simulation.Concerts.Live

[<RequireQualifiedAccess>]
module TuneInstrumentCommand =
    /// Command which allows the player to tune their instrument mid-concert.
    let rec create ongoingConcert =
        Concert.createCommand
            "tune instrument"
            Command.tuneInstrumentDescription
            tuneInstrument
            (fun result points ->
                match result with
                | Done -> Concert.tuneInstrumentDone points
                | _ -> Concert.tooMuchTuning
                |> showMessage)
            ongoingConcert

namespace Duets.Cli.Components.Commands

open Duets.Cli.Components
open Duets.Cli.Components.Commands
open Duets.Cli.Text
open Duets.Simulation.Concerts.Live

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

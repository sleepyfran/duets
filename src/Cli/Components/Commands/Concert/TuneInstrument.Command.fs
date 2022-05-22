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
            CommandTuneInstrumentDescription
            tuneInstrument
            (fun result points ->
                match result with
                | Done -> ConcertTuneInstrumentDone points
                | _ -> ConcertTooMuchTuning
                |> ConcertText
                |> I18n.translate
                |> showMessage)
            ongoingConcert

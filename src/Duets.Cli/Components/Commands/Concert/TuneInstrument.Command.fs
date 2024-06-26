namespace Duets.Cli.Components.Commands

open Duets.Cli.Components.Commands
open Duets.Cli.Text
open Duets.Entities

[<RequireQualifiedAccess>]
module TuneInstrumentCommand =
    /// Command which allows the player to tune their instrument mid-concert.
    let rec create ongoingConcert =
        Concert.eventCommand
            "tune instrument"
            Command.tuneInstrumentDescription
            TuneInstrument
            ongoingConcert

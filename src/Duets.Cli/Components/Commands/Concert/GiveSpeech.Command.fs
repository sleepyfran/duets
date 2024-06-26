namespace Duets.Cli.Components.Commands

open Duets.Cli.Components.Commands
open Duets.Cli.Text
open Duets.Entities

[<RequireQualifiedAccess>]
module GiveSpeechCommand =
    /// Command which simulates giving a speech during a concert.
    let rec create ongoingConcert =
        Concert.eventCommand
            "give speech"
            Command.giveSpeechDescription
            GiveSpeech
            ongoingConcert

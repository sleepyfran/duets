namespace Duets.Cli.Components.Commands

open Duets.Cli.Text
open Duets.Entities

[<RequireQualifiedAccess>]
module GuitarSoloCommand =
    /// Command to perform a guitar solo.
    let create =
        Concert.soloCommand
            "guitar solo"
            Command.guitarSoloDescription
            [ Concert.guitarSoloPlayingReallyFast
              Concert.guitarSoloPlayingWithTeeth
              Concert.guitarSoloDoingSomeTapping ]
            Guitar

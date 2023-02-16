namespace Duets.Cli.Components.Commands

open Duets.Cli.Text
open Duets.Simulation.Concerts.Live

[<RequireQualifiedAccess>]
module GuitarSoloCommand =
    /// Command to perform a guitar solo.
    let create ongoingConcert =
        Concert.createSoloCommand
            "guitar solo"
            Command.guitarSoloDescription
            [ Concert.guitarSoloPlayingReallyFast
              Concert.guitarSoloPlayingWithTeeth
              Concert.guitarSoloDoingSomeTapping ]
            guitarSolo
            ongoingConcert

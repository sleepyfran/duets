namespace Cli.Components.Commands

open Cli.Text
open Simulation
open Simulation.Concerts.Live

[<RequireQualifiedAccess>]
module DrumSoloCommand =
    /// Command to perform a drum solo.
    let create ongoingConcert =
        Concert.createSoloCommand
            "drum solo"
            Command.drumSoloDescription
            [ Concert.drumSoloDoingDrumstickTricks
              Concert.drumSoloPlayingWeirdRhythms
              Concert.drumSoloPlayingReallyFast ]
            drumSolo
            ongoingConcert

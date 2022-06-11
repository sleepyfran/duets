namespace Cli.Components.Commands

open Cli.Text
open Simulation
open Simulation.Concerts.Live

[<RequireQualifiedAccess>]
module BassSoloCommand =
    /// Command to perform a bass solo.
    let create ongoingConcert =
        Concert.createSoloCommand
            "bass solo"
            Command.bassSoloDescription
            [ Concert.bassSoloMovingFingersQuickly
              Concert.bassSoloSlappingThatBass
              Concert.bassSoloGrooving ]
            bassSolo
            ongoingConcert

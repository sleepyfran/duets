namespace Duets.Cli.Components.Commands

open Duets.Cli.Text
open Duets.Simulation.Concerts.Live

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

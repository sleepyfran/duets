namespace Duets.Cli.Components.Commands

open Duets.Cli.Text
open Duets.Entities

[<RequireQualifiedAccess>]
module DrumSoloCommand =
    /// Command to perform a drum solo.
    let create =
        Concert.soloCommand
            "drum solo"
            Command.drumSoloDescription
            [ Concert.drumSoloDoingDrumstickTricks
              Concert.drumSoloPlayingWeirdRhythms
              Concert.drumSoloPlayingReallyFast ]
            Drums

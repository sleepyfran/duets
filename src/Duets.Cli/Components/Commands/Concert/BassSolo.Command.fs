namespace Duets.Cli.Components.Commands

open Duets.Cli.Text
open Duets.Entities

[<RequireQualifiedAccess>]
module BassSoloCommand =
    /// Command to perform a bass solo.
    let create =
        Concert.soloCommand
            "bass solo"
            Command.bassSoloDescription
            [ Concert.bassSoloMovingFingersQuickly
              Concert.bassSoloSlappingThatBass
              Concert.bassSoloGrooving ]
            Bass

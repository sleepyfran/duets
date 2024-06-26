namespace Duets.Cli.Components.Commands

open Duets.Cli.Components.Commands
open Duets.Cli.Text
open Duets.Entities

[<RequireQualifiedAccess>]
module SpinDrumsticksCommand =
    /// Command which performs the action of spinning the drumsticks for drummers.
    let rec create ongoingConcert =
        Concert.eventCommand
            "spin drumstick"
            Command.makeCrowdSingDescription
            SpinDrumsticks
            ongoingConcert

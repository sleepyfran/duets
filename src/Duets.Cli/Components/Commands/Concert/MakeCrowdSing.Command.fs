namespace Duets.Cli.Components.Commands

open Duets.Cli.Components
open Duets.Cli.Components.Commands
open Duets.Cli.Text
open Duets.Entities
open Duets.Simulation.Concerts.Live

[<RequireQualifiedAccess>]
module MakeCrowdSingCommand =
    /// Command which allows the user make the crowd sing.
    let rec create ongoingConcert =
        Concert.eventCommand
            "make crowd sing"
            Command.makeCrowdSingDescription
            MakeCrowdSing
            ongoingConcert

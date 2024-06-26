namespace Duets.Cli.Components.Commands

open Duets.Cli.Components
open Duets.Cli.Components.Commands
open Duets.Cli.Text
open Duets.Entities
open Duets.Simulation.Concerts.Live.Encore

[<RequireQualifiedAccess>]
module GetOffStageCommand =
    /// Command which moves the person from the stage into the backstage. This
    /// might end the concert if people is not really interested in staying for
    /// the encore.
    let rec create =
        Concert.eventCommand
            "get off stage"
            Command.getOffStageDescription
            GetOffStage

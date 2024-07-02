namespace Duets.Cli.Components.Commands

open Duets.Cli.Components.Commands
open Duets.Cli.Text
open Duets.Entities

[<RequireQualifiedAccess>]
module FinishConcertCommand =
    /// Puts the artist out of the ongoing concert scene, which shows them the
    /// total points accumulated during the concert, the result of it and allows
    /// them to move to other places outside the stage/backstage.
    let rec create ongoingConcert =
        Command.action
            "finish concert"
            Command.finishConcertDescription
            (ConcertFinish ongoingConcert)

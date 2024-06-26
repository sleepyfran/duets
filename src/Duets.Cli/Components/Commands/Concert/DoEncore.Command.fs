namespace Duets.Cli.Components.Commands

open Duets.Cli.Components.Commands
open Duets.Cli.Text
open Duets.Entities

[<RequireQualifiedAccess>]
module DoEncoreCommand =
    /// Returns the artist back to the stage to perform an encore. Assumes that
    /// an encore is possible and that the audience will still be there for it.
    let create ongoingConcert =
        Command.action
            "do encore"
            Command.doEncoreDescription
            (ConcertEncore ongoingConcert)

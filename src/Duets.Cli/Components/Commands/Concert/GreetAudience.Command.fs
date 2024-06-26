namespace Duets.Cli.Components.Commands

open Duets.Cli.Components.Commands
open Duets.Cli.Text
open Duets.Entities

[<RequireQualifiedAccess>]
module GreetAudienceCommand =
    /// Command which greets the audience in the concert.
    let rec create ongoingConcert =
        Concert.eventCommand
            "greet audience"
            Command.greetAudienceDescription
            GreetAudience
            ongoingConcert

namespace Duets.Cli.Components.Commands

open Duets.Cli.Components
open Duets.Cli.Components.Commands
open Duets.Cli.Text
open Duets.Simulation.Concerts.Live

[<RequireQualifiedAccess>]
module GreetAudienceCommand =
    /// Command which greets the audience in the concert.
    let rec create ongoingConcert =
        Concert.createCommand
            "greet audience"
            Command.greetAudienceDescription
            greetAudience
            (fun result points ->
                match result with
                | TooManyRepetitionsPenalized
                | TooManyRepetitionsNotDone ->
                    Concert.greetAudienceGreetedMoreThanOnceTip points
                | _ -> Concert.greetAudienceDone points
                |> showMessage)
            ongoingConcert

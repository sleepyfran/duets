namespace Cli.Components.Commands

open Cli.Components
open Cli.Components.Commands
open Cli.Text
open Simulation
open Simulation.Concerts.Live

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

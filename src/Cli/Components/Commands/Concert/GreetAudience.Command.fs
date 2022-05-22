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
            CommandGreetAudienceDescription
            greetAudience
            (fun result points ->
                match result with
                | TooManyRepetitionsPenalized
                | TooManyRepetitionsNotDone ->
                    ConcertGreetAudienceGreetedMoreThanOnceTip points
                | _ -> ConcertGreetAudienceDone points
                |> ConcertText
                |> I18n.translate
                |> showMessage)
            ongoingConcert

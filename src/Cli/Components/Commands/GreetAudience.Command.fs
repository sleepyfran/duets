namespace Cli.Components.Commands

open Agents
open Cli.Components
open Cli.Components.Commands
open Cli.SceneIndex
open Cli.Text
open Simulation
open Simulation.Concerts.Live

[<RequireQualifiedAccess>]
module GreetAudienceCommand =
    /// Command which greets the audience in the concert and calls `concertScene`
    /// with the result.
    let rec create ongoingConcert =
        { Name = "greet audience"
          Description =
            ConcertText ConcertCommandPlayDescription
            |> I18n.translate
          Handler =
            (fun _ ->
                let response =
                    greetAudience (State.get ()) ongoingConcert

                match response.Result with
                | TooManyRepetitionsPenalized
                | TooManyRepetitionsNotDone ->
                    ConcertGreetAudienceGreetedMoreThanOnceTip response.Points
                | _ -> ConcertGreetAudienceDone response.Points
                |> ConcertText
                |> I18n.translate
                |> showMessage

                response.OngoingConcert
                |> Situations.inConcert
                |> Cli.Effect.apply

                Scene.World) }

namespace Cli.Scenes.InteractiveSpaces.ConcertSpace.Commands

open Cli.Components
open Cli.Components.Commands
open Cli.Text
open Simulation
open Simulation.Concerts.Live

[<RequireQualifiedAccess>]
module GreetAudienceCommand =
    /// Command which greets the audience in the concert and calls `concertScene`
    /// with the result.
    let rec create ongoingConcert concertScene =
        { Name = "greet audience"
          Description =
              ConcertText ConcertCommandPlayDescription
              |> I18n.translate
          Handler =
              (fun _ ->
                  let response = greetAudience ongoingConcert

                  match response.Result with
                  | GreetedMoreThanOnce ->
                      ConcertGreetAudienceGreetedMoreThanOnceTip response.Points
                  | Ok -> ConcertGreetAudienceDone response.Points
                  |> ConcertText
                  |> I18n.translate
                  |> showMessage

                  response.OngoingConcert |> concertScene |> Some) }

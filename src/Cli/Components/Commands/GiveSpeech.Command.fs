namespace Cli.Components.Commands

open Agents
open Cli.Components
open Cli.Components.Commands
open Cli.SceneIndex
open Cli.Text
open Simulation
open Simulation.Concerts.Live

[<RequireQualifiedAccess>]
module GiveSpeechCommand =
    /// Command which simulates giving a speech during a concert.
    let rec create ongoingConcert =
        { Name = "give speech"
          Description =
            ConcertText ConcertCommandPlayDescription
            |> I18n.translate
          Handler =
            (fun _ ->
                Common.showSpeechProgress ()

                let response =
                    giveSpeech (State.get ()) ongoingConcert

                match response.Result with
                | LowPerformance -> ConcertSpeechGivenLowSkill response.Points
                | AveragePerformance ->
                    ConcertSpeechGivenMediumSkill response.Points
                | GoodPerformance
                | GreatPerformance ->
                    ConcertSpeechGivenHighSkill response.Points
                | _ -> ConcertTooManySpeeches
                |> ConcertText
                |> I18n.translate
                |> showMessage

                response.OngoingConcert
                |> Situations.inConcert
                |> Cli.Effect.apply

                Scene.World) }

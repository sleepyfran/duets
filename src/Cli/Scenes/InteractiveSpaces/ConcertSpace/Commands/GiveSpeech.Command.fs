namespace Cli.Scenes.InteractiveSpaces.ConcertSpace.Commands

open Agents
open Cli.Components
open Cli.Components.Commands
open Cli.Text
open Simulation
open Simulation.Concerts.Live

[<RequireQualifiedAccess>]
module GiveSpeechCommand =
    /// Command which simulates giving a speech during a concert.
    let rec create ongoingConcert concertScene =
        { Name = "give speech"
          Description =
              ConcertText ConcertCommandPlayDescription
              |> I18n.translate
          Handler =
              (fun _ ->
                  let response = giveSpeech (State.get ()) ongoingConcert

                  match response.Result with
                  | LowSpeechSkill -> ConcertSpeechGivenLowSkill response.Points
                  | MediumSpeechSkill ->
                      ConcertSpeechGivenMediumSkill response.Points
                  | HighSpeechSkill ->
                      ConcertSpeechGivenHighSkill response.Points
                  | TooManySpeeches -> ConcertTooManySpeeches
                  |> ConcertText
                  |> I18n.translate
                  |> showMessage

                  response.OngoingConcert |> concertScene) }

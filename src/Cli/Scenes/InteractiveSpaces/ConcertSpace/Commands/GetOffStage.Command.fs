namespace Cli.Scenes.InteractiveSpaces.ConcertSpace.Commands

open Agents
open Cli.Components
open Cli.Components.Commands
open Cli.SceneIndex
open Cli.Text
open Entities
open Simulation.Concerts.Live

[<RequireQualifiedAccess>]
module GetOffStageCommand =
    /// Command which moves the person from the stage into the backstage. This
    /// might end the concert if people is not really interested in staying for
    /// the encore.
    let rec create ongoingConcert backstageScene =
        { Name = "get off stage"
          Description =
              ConcertText ConcertCommandGetOffStageDescription
              |> I18n.translate
          Handler =
              (fun _ ->
                  let response =
                      Encore.getOffStage (State.get ()) ongoingConcert

                  response.Effects |> List.iter Cli.Effect.apply

                  let canPerformEncore, backstageRoomId = response.Result

                  lineBreak ()

                  if canPerformEncore then
                      ConcertText ConcertGetOffStageEncorePossible
                      |> I18n.translate
                      |> showMessage

                      lineBreak ()

                      backstageScene backstageRoomId response.OngoingConcert
                  else
                      ConcertText ConcertGetOffStageNoEncorePossible
                      |> I18n.translate
                      |> showMessage

                      lineBreak ()

                      Scene.World) }

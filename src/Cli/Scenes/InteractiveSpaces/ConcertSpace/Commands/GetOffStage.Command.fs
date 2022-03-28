namespace Cli.Scenes.InteractiveSpaces.ConcertSpace.Commands

open Agents
open Cli.Components
open Cli.Components.Commands
open Cli.SceneIndex
open Cli.Text
open Entities
open Simulation

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
                  let backstageCoordinates =
                      Queries.World.ConcertSpace.closestBackstage (State.get ())
                      |> Option.get // Not having a backstage is a problem in city creation.

                  State.get ()
                  |> World.Navigation.moveTo backstageCoordinates
                  |> Cli.Effect.apply

                  lineBreak ()

                  if Concert.Ongoing.canPerformEncore ongoingConcert then
                      ConcertText ConcertGetOffStageEncorePossible
                      |> I18n.translate
                      |> showMessage

                      lineBreak ()

                      let backstageRoomId =
                          Queries.World.Common.roomIdFromCoordinates
                              backstageCoordinates
                          |> Option.get

                      backstageScene backstageRoomId ongoingConcert
                      |> Some
                  else
                      ConcertText ConcertGetOffStageNoEncorePossible
                      |> I18n.translate
                      |> showMessage

                      lineBreak ()

                      Concerts.Live.Finish.finishConcert
                          (State.get ())
                          ongoingConcert
                      |> Cli.Effect.apply

                      Some Scene.World) }

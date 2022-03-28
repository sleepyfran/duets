namespace Cli.Scenes.InteractiveSpaces.ConcertSpace.Commands

open Agents
open Cli.Components
open Cli.Components.Commands
open Cli.Text
open Simulation

[<RequireQualifiedAccess>]
module DoEncoreCommand =
    /// Returns the artist back to the stage to perform an encore. Assumes that
    /// an encore is possible and that the audience will still be there for it.
    let rec create ongoingConcert concertScene =
        { Name = "do encore"
          Description =
              ConcertText ConcertCommandDoEncoreDescription
              |> I18n.translate
          Handler =
              (fun _ ->
                  let stageCoordinates =
                      Queries.World.ConcertSpace.closestStage (State.get ())
                      |> Option.get // Not having a stage is a problem in city creation.

                  State.get ()
                  |> World.Navigation.moveTo stageCoordinates
                  |> Cli.Effect.apply

                  lineBreak ()

                  ConcertText ConcertEncoreComingBackToStage
                  |> I18n.translate
                  |> showMessage

                  concertScene ongoingConcert |> Some) }

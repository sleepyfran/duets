namespace Cli.Components.Commands

open Agents
open Cli.Components
open Cli.Components.Commands
open Cli.SceneIndex
open Cli.Text
open Simulation

[<RequireQualifiedAccess>]
module DoEncoreCommand =
    /// Returns the artist back to the stage to perform an encore. Assumes that
    /// an encore is possible and that the audience will still be there for it.
    let create ongoingConcert =
        { Name = "do encore"
          Description =
            ConcertText ConcertCommandDoEncoreDescription
            |> I18n.translate
          Handler =
            (fun _ ->
                let encoreResponse =
                    Concerts.Live.Encore.doEncore (State.get ()) ongoingConcert

                encoreResponse.Effects
                |> List.iter Cli.Effect.apply

                lineBreak ()

                ConcertText ConcertEncoreComingBackToStage
                |> I18n.translate
                |> showMessage

                encoreResponse.OngoingConcert
                |> Situations.inConcert
                |> Cli.Effect.apply

                Scene.World) }

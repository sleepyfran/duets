namespace Cli.Components.Commands

open Agents
open Cli.Components.Commands
open Cli.SceneIndex
open Cli.Text
open Simulation

[<RequireQualifiedAccess>]
module FinishConcertCommand =
    /// Puts the artist out of the ongoing concert scene, which shows them the
    /// total points accumulated during the concert, the result of it and allows
    /// them to move to other places outside the stage/backstage.
    let rec create ongoingConcert =
        { Name = "finish concert"
          Description = Command.finishConcertDescription
          Handler =
            (fun _ ->
                Concerts.Live.Finish.finishConcert (State.get ()) ongoingConcert
                |> Cli.Effect.applyMultiple

                Scene.World) }

namespace Cli.Components.Commands

open Agents
open Cli
open Cli.Components.Commands
open Cli.SceneIndex
open Cli.Text
open Simulation
open Simulation.Concerts

[<RequireQualifiedAccess>]
module StartConcertCommand =
    /// Returns a command that starts a scheduled concert.
    let create concertSpaceId =
        { Name = "start concert"
          Description = Command.startConcertDescription
          Handler =
            fun _ ->
                Scheduler.startScheduledConcerts (State.get ()) concertSpaceId
                |> Effect.applyMultiple

                Scene.World }

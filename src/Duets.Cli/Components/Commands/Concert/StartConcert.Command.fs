namespace Duets.Cli.Components.Commands

open Duets.Agents
open Duets.Cli
open Duets.Cli.Components.Commands
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Entities
open Duets.Simulation

[<RequireQualifiedAccess>]
module StartConcertCommand =
    /// Returns a command that starts a scheduled concert.
    let create scheduledConcert =
        { Name = "start concert"
          Description = Command.startConcertDescription
          Handler =
            fun _ ->
                let currentBand = Queries.Bands.currentBand (State.get ())

                ConcertStart
                    {| Band = currentBand
                       Concert = scheduledConcert |}
                |> Effect.applyAction

                Scene.World }

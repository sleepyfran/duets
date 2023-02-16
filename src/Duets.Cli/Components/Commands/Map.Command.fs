namespace Duets.Cli.Components.Commands

open Duets.Agents
open Duets.Cli
open Duets.Cli.Components
open Duets.Cli.Text
open Duets.Cli.SceneIndex
open Duets.Entities
open Duets.Simulation

[<RequireQualifiedAccess>]
module rec MapCommand =
    /// Creates a command that allows the player to navigate to different
    /// parts of the game world inside the current city.
    let get =
        { Name = "map"
          Description = Command.mapDescription
          Handler =
            fun _ ->
                Queries.World.currentCity (State.get ())
                |> fun city -> city.Id
                |> Command.mapCurrentCity
                |> showMessage

                Command.mapTip |> showMessage
                lineBreak ()

                showMap () |> Effect.applyMultiple

                lineBreak ()

                Scene.WorldAfterMovement }

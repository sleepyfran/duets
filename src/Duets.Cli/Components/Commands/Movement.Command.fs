namespace Duets.Cli.Components.Commands

open Duets.Agents
open Duets.Cli
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text.World
open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Navigation

[<RequireQualifiedAccess>]
module MovementCommand =
    /// Creates a set of commands with the available direction as the name which,
    /// when executed, moves the player towards that direction.
    let create direction roomId =
        let commandName =
            match direction with
            | North -> "n"
            | NorthEast -> "ne"
            | East -> "e"
            | SouthEast -> "se"
            | South -> "s"
            | SouthWest -> "sw"
            | West -> "w"
            | NorthWest -> "nw"

        { Name = commandName
          Description =
            $"Allows you to move to the {World.directionName direction}"
          Handler =
            fun _ ->
                $"You make your way to the {World.directionName direction}..."
                |> showMessage

                wait 1000<millisecond>

                State.get () |> Navigation.enter roomId |> Effect.apply
                Scene.WorldAfterMovement }

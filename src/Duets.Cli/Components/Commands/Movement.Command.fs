namespace Duets.Cli.Components.Commands

open Duets.Agents
open Duets.Cli
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Navigation

[<RequireQualifiedAccess>]
module MovementCommand =
    let private directionName direction =
        match direction with
        | North -> "north"
        | NorthEast -> "north-east"
        | East -> "east"
        | SouthEast -> "south-east"
        | South -> "south"
        | SouthWest -> "south-west"
        | West -> "west"
        | NorthWest -> "north-west"

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
          Description = $"Allows you to move to the {directionName direction}"
          Handler =
            fun _ ->
                $"You make your way to the {directionName direction}..."
                |> showMessage

                wait 1000<millisecond>

                State.get () |> Navigation.enter roomId |> Effect.apply
                Scene.WorldAfterMovement }

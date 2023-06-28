namespace Duets.Cli.Components.Commands

open Duets.Agents
open Duets.Cli
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
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

                let result = State.get () |> Navigation.enter roomId

                match result with
                | Ok effect -> Effect.apply effect
                | Error RoomEntranceError.CannotEnterStageOutsideConcert ->
                    $"""Initially the people in the bar were looking weirdly at you thinking what were you doing in there. Then the {Styles.person "bouncer"} came and kicked you out warning you {Styles.danger
                                                                                                                                                                                                       "not to get in the stage again if you're not part of the band playing"}"""
                    |> showMessage
                | Error RoomEntranceError.CannotEnterBackstageOutsideConcert ->
                    $"""You tried to sneak into the {Styles.place "backstage"}, but the bouncers catch you as soon as you enter and kicked you out warning you {Styles.danger "not to enter in there if you're not part of the band playing"}"""
                    |> showMessage

                Scene.WorldAfterMovement }

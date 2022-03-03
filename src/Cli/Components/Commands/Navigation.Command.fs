namespace Cli.Components.Commands

open Agents
open Cli
open Cli.SceneIndex
open Cli.Text
open Entities
open Simulation

[<RequireQualifiedAccess>]
module NavigationCommand =
    let northCommand = "n"
    let northEastCommand = "ne"
    let eastCommand = "e"
    let southEastCommand = "se"
    let southCommand = "s"
    let southWestCommand = "sw"
    let westCommand = "w"
    let northWestCommand = "nw"

    let private handle coordinates _ =
        State.get ()
        |> World.Navigation.moveTo coordinates
        |> Effect.apply

        Some Scene.World

    /// Creates a set of commands with the available direction as the name which,
    /// when executed, moves the player towards that direction.
    let create entrances =
        entrances
        |> List.map
            (fun (direction, _, coordinates) ->
                let commandName =
                    match direction with
                    | North -> northCommand
                    | NorthEast -> northEastCommand
                    | East -> eastCommand
                    | SouthEast -> southEastCommand
                    | South -> southCommand
                    | SouthWest -> southWestCommand
                    | West -> westCommand
                    | NorthWest -> northWestCommand

                { Name = commandName
                  Description =
                      CommandDirectionDescription direction
                      |> CommandText
                      |> I18n.translate
                  Handler = handle coordinates })

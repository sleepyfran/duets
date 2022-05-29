namespace Cli.Components.Commands

open Agents
open Common
open Cli.Components
open Cli.SceneIndex
open Cli.Text
open Entities
open Simulation
open Simulation.Navigation

[<RequireQualifiedAccess>]
module NavigationCommand =
    let private northCommand = "n"
    let private northEastCommand = "ne"
    let private eastCommand = "e"
    let private southEastCommand = "se"
    let private southCommand = "s"
    let private southWestCommand = "sw"
    let private westCommand = "w"
    let private northWestCommand = "nw"

    let private handle coordinates _ =
        State.get ()
        |> Navigation.moveTo coordinates
        |> Result.switch Cli.Effect.apply Common.showEntranceError

        Scene.World

    /// Creates a set of commands with the available direction as the name which,
    /// when executed, moves the player towards that direction.
    let create direction coordinates =
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
          Handler = handle coordinates }

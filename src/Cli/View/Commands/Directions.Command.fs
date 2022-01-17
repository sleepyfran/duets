namespace Cli.View.Commands

open Agents
open Cli.View.Actions
open Cli.View.Text
open Entities
open Simulation

[<RequireQualifiedAccess>]
module DirectionsCommand =
    /// Creates a set of commands with the available direction as the name which,
    /// when executed, moves the player towards that direction.
    let create entrances =
        entrances
        |> List.map
            (fun (direction, linkedNodeId) ->
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
                      CommandDirectionDescription direction
                      |> CommandText
                      |> I18n.translate
                  Handler =
                      HandlerWithNavigation
                          (fun _ ->
                              seq {
                                  yield
                                      State.get ()
                                      |> World.Navigation.moveTo linkedNodeId
                                      |> Effect

                                  yield Scene Scene.World
                              }) })

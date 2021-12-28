namespace Cli.View.Commands

open Cli.View.Actions
open Cli.View.TextConstants
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
                    | North -> "north"
                    | NorthEast -> "north-east"
                    | East -> "east"
                    | SouthEast -> "south-east"
                    | South -> "south"
                    | SouthWest -> "south-west"
                    | West -> "west"
                    | NorthWest -> "north-west"

                { Name = commandName
                  Description =
                      TextConstant
                      <| CommandDirectionDescription direction
                  Handler =
                      HandlerWithNavigation
                          (fun _ ->
                              seq {
                                  yield
                                      State.Root.get ()
                                      |> World.Navigation.moveTo linkedNodeId
                                      |> Effect

                                  yield Scene Scene.World
                              }) })

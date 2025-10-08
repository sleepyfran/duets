namespace Duets.Cli.Components.Commands

open Duets.Cli
open Duets.Cli.SceneIndex
open Duets.Simulation

[<RequireQualifiedAccess>]
module LeaveMetroCommand =
    /// Command to leave the metro onto the platform.
    let get =
        { Name = "leave"
          Description = "Allows you leave the metro onto the platform."
          Handler =
            fun _ ->
                Situations.freeRoam |> Effect.apply

                Scene.WorldAfterMovement }

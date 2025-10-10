namespace Duets.Cli.Components.Commands

open Duets.Cli
open Duets.Cli.SceneIndex
open Duets.Simulation

[<RequireQualifiedAccess>]
module LeaveVehicleCommand =
    /// Command to leave the car or metro.
    let get =
        { Name = "leave"
          Description = "Allows you leave the current vehicle."
          Handler =
            fun _ ->
                Situations.freeRoam |> Effect.apply
                Scene.WorldAfterMovement }

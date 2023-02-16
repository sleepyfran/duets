namespace Duets.Cli.Components.Commands

open Duets.Agents
open Duets.Cli
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Simulation.Careers.Work

[<RequireQualifiedAccess>]
module WorkCommand =
    /// Command that allows the player to start a shift in their work.
    let create job =
        { Name = "work"
          Description = Command.workDescription job
          Handler =
            fun _ ->
                Career.workShiftEvent job |> showMessage

                wait 5000<millisecond>

                let salary, effects = workShift (State.get ()) job

                Career.workShiftFinished salary |> showMessage
                effects |> Effect.applyMultiple

                Scene.WorldAfterMovement }

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

                let result = workShift (State.get ()) job

                match result with
                | Ok effects ->
                    Career.workShiftEvent job |> showMessage
                    wait 5000<millisecond>
                    Effect.applyMultiple effects

                    Scene.WorldAfterMovement
                | Error _ ->
                    "The place is currently close, you can't work now!"
                    |> Styles.error
                    |> showMessage

                    Scene.World

        }

namespace Duets.Cli.Components.Commands

open Duets.Agents
open Duets.Cli
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Microsoft.FSharp.Data.UnitSystems.SI.UnitNames
open Duets.Simulation.Travel

[<RequireQualifiedAccess>]
module WaitForMetroCommand =
    /// Command that allows the user to wait until the next train arrives.
    let get =
        { Name = "wait"
          Description = "Allows you to wait until the next train arrives."
          Handler =
            fun _ ->
                showProgressBarAsync
                    [ Travel.blindlyStaringAtPhone
                      Travel.gettingAnnoyed
                      Travel.waitingSomeMore ]
                    2<second>

                Metro.waitForNextTrain (State.get ()) |> Effect.applyMultiple

                "The rumble of an approaching train grows louder, and the bright headlights of a metro car illuminate the tunnel ahead. With a hiss of air brakes, the train comes to a complete stop before you, doors sliding open. Passengers begin to disembark, and the doors then pause briefly to let you board it."
                |> Styles.faded
                |> showMessage

                Scene.WorldAfterMovement }

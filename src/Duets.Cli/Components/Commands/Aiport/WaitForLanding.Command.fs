namespace Duets.Cli.Components.Commands

open Duets.Cli
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Entities
open Microsoft.FSharp.Data.UnitSystems.SI.UnitNames

[<RequireQualifiedAccess>]
module WaitForLandingCommand =
    /// Command that allows the user to wait until the flight finishes.
    let create flight =
        { Name = "wait"
          Description = Command.waitForLandingDescription
          Handler =
            fun _ ->
                showProgressBarSync
                    [ Airport.waitForLanding
                      Airport.gettingOffPlane
                      Airport.passingPassportControl ]
                    5<second>

                AirportWaitForLanding flight |> Effect.applyAction
                Scene.WorldAfterMovement }

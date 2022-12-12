namespace Cli.Components.Commands

open Agents
open Cli
open Cli.Components
open Cli.SceneIndex
open Cli.Text
open Microsoft.FSharp.Data.UnitSystems.SI.UnitNames
open Simulation.Flights.Airport

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

                leavePlane (State.get ()) flight
                |> Effect.applyMultiple

                Scene.WorldAfterMovement }

namespace Duets.Cli.Components.Commands

open Duets.Agents
open Duets.Cli
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Microsoft.FSharp.Data.UnitSystems.SI.UnitNames
open Duets.Simulation.Flights.Airport

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

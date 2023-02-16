namespace Duets.Cli.Components.Commands

open Duets.Agents
open Duets.Cli
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Entities
open Microsoft.FSharp.Data.UnitSystems.SI.UnitNames
open Duets.Simulation.Flights.Airport

[<RequireQualifiedAccess>]
module BoardPlaneCommand =
    /// Command that allows the user to board a plane they've previously booked.
    let create flight =
        { Name = "board plane"
          Description = Command.boardPlaneDescription flight
          Handler =
            fun _ ->
                showProgressBarSync [ Airport.passingSecurityCheck ] 5<second>

                let takenItemsEffect =
                    passSecurityCheck (State.get ())

                match takenItemsEffect with
                | effects when effects.Length > 0 ->
                    Airport.itemsTakenBySecurity |> showMessage
                    Effect.applyMultiple effects
                | _ -> ()

                showProgressBarAsync [ Airport.waitingToBoard ] 5<second>

                let effects, flightTime = boardPlane flight

                Airport.planeBoarded flight flightTime
                |> showMessage

                effects |> Effect.applyMultiple

                Scene.World }

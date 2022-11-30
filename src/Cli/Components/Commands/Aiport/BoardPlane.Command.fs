namespace Cli.Components.Commands

open Agents
open Cli
open Cli.Components
open Cli.SceneIndex
open Cli.Text
open Entities
open Microsoft.FSharp.Data.UnitSystems.SI.UnitNames
open Simulation.Flights.Airport

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

                let effect, flightTime = boardPlane flight

                Airport.planeBoarded flight flightTime
                |> showMessage

                Effect.apply effect

                Scene.World }

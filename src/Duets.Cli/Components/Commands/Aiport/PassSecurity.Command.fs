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
module PassSecurityCommand =
    /// Command that allows the user to pass the security check in the airport.
    let get =
        { Name = "pass security check"
          Description = Command.passSecurityCheckDescription
          Handler =
            fun _ ->
                showProgressBarSync [ Travel.passingSecurityCheck ] 5<second>

                let effects = passSecurityCheck (State.get ())
                Effect.applyMultiple effects

                let takenItemsEffect =
                    effects
                    |> List.filter (function
                        | ItemRemovedFromCharacterInventory _ -> true
                        | _ -> false)

                match takenItemsEffect with
                | eff when eff.Length > 0 ->
                    Travel.itemsTakenBySecurity |> showMessage
                | _ -> ()

                Scene.WorldAfterMovement }

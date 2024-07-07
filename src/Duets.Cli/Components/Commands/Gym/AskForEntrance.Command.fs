namespace Duets.Cli.Components.Commands

open Duets.Agents
open Duets.Cli
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Simulation.Bank.Operations
open Duets.Simulation.Gym

[<RequireQualifiedAccess>]
module AskForEntranceCommand =
    /// Command to pay for a one-time entrance to a gym.
    let create entranceFee =
        { Name = "ask for entrance"
          Description = "Allows you to pay for a one-time entrance to a gym"
          Handler =
            (fun _ ->
                let confirmed =
                    $"The entrance fee is {Styles.money entranceFee}. Do you want to pay it?"
                    |> showConfirmationPrompt

                if confirmed then
                    let result = Entrance.pay (State.get ()) entranceFee

                    match result with
                    | Ok effects -> effects |> Effect.applyMultiple
                    | Error(NotEnoughFunds _) ->
                        Shop.notEnoughFunds |> showMessage

                Scene.World) }

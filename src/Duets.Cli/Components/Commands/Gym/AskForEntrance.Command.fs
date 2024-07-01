namespace Duets.Cli.Components.Commands

open Duets.Cli
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Entities

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
                    GymPayEntranceFee entranceFee |> Effect.applyAction

                Scene.World) }

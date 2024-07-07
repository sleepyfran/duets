namespace Duets.Cli.Components.Commands

open Duets.Agents
open Duets.Cli
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Entities
open Duets.Simulation.MiniGames

[<RequireQualifiedAccess>]
module BetCommand =
    /// Command which allows the player to bet to start the game.
    let create miniGameState =
        { Name = "bet"
          Description =
            "Allows you to bet on the blackjack table and start the game"
          Handler =
            (fun _ ->
                let amount =
                    showRangedDecimalPrompt
                        0m
                        100m
                        "How much would you like to bet?"
                    |> Amount.fromDecimal

                let result = Blackjack.bet (State.get ()) miniGameState amount

                match result with
                | Ok effects -> effects |> Effect.apply
                | Error Blackjack.NotEnoughFunds ->
                    "You don't have enough funds to bet that much."
                    |> Styles.error
                    |> showMessage
                | Error Blackjack.InvalidBet ->
                    "That bet is invalid." |> Styles.error |> showMessage
                | Error Blackjack.BettingNotAllowed ->
                    "You can't bet right now." |> Styles.error |> showMessage

                Scene.World) }

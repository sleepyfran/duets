namespace Duets.Cli.Components.Commands

open Duets.Agents
open Duets.Cli
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Simulation.MiniGames

[<RequireQualifiedAccess>]
module StandCommand =
    let private showGameSummary game =
        let dealerHand = MiniGame.dealerFullHand game
        $"Dealer hand:  {dealerHand}" |> Styles.faded |> showMessage

        $"Dealer value: {game.DealerHand.Score |> MiniGame.score}"
        |> Styles.faded
        |> showMessage

        let playerHand = MiniGame.playerHand game
        $"Your hand:    {playerHand}" |> Styles.faded |> showMessage

        $"Total value:  {game.PlayerHand.Score |> MiniGame.score}"
        |> Styles.faded
        |> showMessage

    /// Command which allows the player to end their turn.
    let create miniGameState =
        { Name = "stand"
          Description =
            "Allows you to stay with your current hand and end your turn"
          Handler =
            (fun _ ->
                let result = Blackjack.stand (State.get ()) miniGameState

                match result with
                | Ok(Blackjack.DealerWin(game, effects)) ->
                    showGameSummary game

                    $"The dealer wins! You lost {game.Bet |> Styles.money}"
                    |> Styles.error
                    |> showMessage

                    effects |> Effect.applyMultiple
                | Ok(Blackjack.DealerBusted(game, effects)) ->
                    showGameSummary game

                    $"The dealer busted! You win {game.Bet |> Styles.money}"
                    |> Styles.success
                    |> showMessage

                    effects |> Effect.applyMultiple
                | Ok(Blackjack.Push(game, effects)) ->
                    showGameSummary game

                    "Both you and the dealer got the same value! You get your bet back"
                    |> Styles.success
                    |> showMessage

                    effects |> Effect.applyMultiple
                | Ok(Blackjack.PlayerBlackjack(game, total, effects)) ->
                    showGameSummary game

                    $"You got a blackjack! You win {total |> Styles.money}"
                    |> Styles.success
                    |> showMessage

                    effects |> Effect.applyMultiple
                | Ok(Blackjack.PlayerWin(game, effects)) ->
                    showGameSummary game

                    $"You win! You earned {game.Bet |> Styles.money}"
                    |> Styles.success
                    |> showMessage

                    effects |> Effect.applyMultiple
                | Error Blackjack.NotAllowed ->
                    "You can't stand right now!" |> Styles.error |> showMessage

                Scene.World) }

namespace Duets.Cli.Components.Commands

open Duets.Agents
open Duets.Cli
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Simulation.MiniGames

[<RequireQualifiedAccess>]
module HitCommand =
    let private showGameSummary game =
        let playerHand = MiniGame.playerHand game
        $"Your hand:   {playerHand}" |> Styles.faded |> showMessage

        $"Total value: {game.PlayerHand.Score |> MiniGame.score}"
        |> Styles.faded
        |> showMessage

    /// Command which allows the player to ask for another card.
    let create miniGameState =
        { Name = "hit"
          Description = "Allows you to ask for another card"
          Handler =
            (fun _ ->
                let result = Blackjack.hit (State.get ()) miniGameState

                match result with
                | Ok(Blackjack.Bust(game, effects)) ->
                    showGameSummary game

                    $"You busted! You lost {game.Bet |> Styles.money}"
                    |> Styles.error
                    |> showMessage

                    effects |> Effect.applyMultiple
                | Ok(Blackjack.Continue effects) ->
                    effects |> Effect.applyMultiple
                | Error Blackjack.NotAllowed ->
                    "You can't hit right now!" |> Styles.error |> showMessage

                Scene.World) }

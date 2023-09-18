namespace Duets.Cli.Components.Commands

open Duets.Agents
open Duets.Cli
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Simulation.MiniGames

[<RequireQualifiedAccess>]
module LeaveCommand =
    /// Command which allows the player to leave the current mini-game.
    let create miniGameId miniGameState =
        { Name = "leave"
          Description =
            $"Allows you to leave the {Generic.miniGameName miniGameId} game"
          Handler =
            (fun _ ->
                let result = Blackjack.leave (State.get ()) miniGameState

                match result with
                | Ok effect -> effect |> Effect.applyMultiple
                | Error Blackjack.NotAllowed ->
                    "You can't leave the game now!"
                    |> Styles.error
                    |> showMessage

                Scene.World) }

namespace Duets.Cli.Components.Commands

open Duets.Cli
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Simulation

[<RequireQualifiedAccess>]
module StartMiniGameCommand =
    /// Command which starts a mini-game given its ID.
    let create miniGameId =
        { Name = $"play {Generic.miniGameName miniGameId}"
          Description =
            $"Allows you to start a game of {Generic.miniGameName miniGameId}"
          Handler =
            (fun _ ->
                MiniGames.Blackjack.startGame |> Effect.apply

                Scene.World) }

namespace Duets.Simulation.Queries.Internal.Interactions

open Duets.Entities
open Duets.Entities.SituationTypes
open Duets.Simulation

module Casino =
    let private blackjackBettingInteractions miniGameState =
        [ MiniGameInGameInteraction.Bet miniGameState
          MiniGameInGameInteraction.Leave(MiniGameId.Blackjack, miniGameState) ]
        |> List.map (MiniGameInteraction.InGame >> Interaction.MiniGame)

    let private blackjackInGameInteractions miniGameState =
        [ MiniGameInGameInteraction.Hit miniGameState
          MiniGameInGameInteraction.Stand miniGameState ]
        |> List.map (MiniGameInteraction.InGame >> Interaction.MiniGame)

    let private blackjackInteractions gameState =
        match gameState with
        | Betting -> blackjackBettingInteractions gameState
        | Playing _ -> blackjackInGameInteractions gameState

    let private startGameInteractions =
        [ MiniGameId.Blackjack ]
        |> List.map (MiniGameInteraction.StartGame >> Interaction.MiniGame)

    let internal interactions state roomType defaultInteractions =
        let situation = Queries.Situations.current state

        match roomType, situation with
        | RoomType.CasinoFloor, PlayingMiniGame(Blackjack miniGameState) ->
            blackjackInteractions miniGameState
        | RoomType.CasinoFloor, _ -> startGameInteractions @ defaultInteractions
        | _ -> defaultInteractions

module Duets.Simulation.MiniGames.Blackjack

open Duets.Common
open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Bank.Operations
open Duets.Simulation.Time

/// Starts a new blackjack game by setting the current situation to playing
/// a mini-game of blackjack.
let startGame = Betting |> Blackjack |> Situations.playingMiniGame

type BetError =
    | NotEnoughFunds
    | InvalidBet
    | BettingNotAllowed

let private dealCard () =
    let randomCardIdx = RandomGen.sampleIndex MiniGame.allCards
    List.item randomCardIdx MiniGame.allCards

let private dealCards () = [ dealCard (); dealCard () ]

let private computeScore card =
    match card.Rank with
    | Ace -> ScoreType.Multiple(1, 11)
    | Two -> ScoreType.Single 2
    | Three -> ScoreType.Single 3
    | Four -> ScoreType.Single 4
    | Five -> ScoreType.Single 5
    | Six -> ScoreType.Single 6
    | Seven -> ScoreType.Single 7
    | Eight -> ScoreType.Single 8
    | Nine -> ScoreType.Single 9
    | Ten -> ScoreType.Single 10
    | Jack -> ScoreType.Single 10
    | Queen -> ScoreType.Single 10
    | King -> ScoreType.Single 10

/// Returns the score of a hand. If the hand only has one possible score, that
/// score is returned. If the hand has multiple possible scores, the lowest
/// possible score is returned, unless each score is under 21, in which case
/// the highest possible score is returned.
let scoreValue scoreType =
    match scoreType with
    | ScoreType.Single x -> x
    | ScoreType.Multiple(x, y) ->
        match x, y with
        | x, y when x <= 21 && y <= 21 -> max x y
        | _ -> min x y

let private sumScores scores =
    scores
    |> List.reduce (fun prevScore currScore ->
        match prevScore, currScore with
        | ScoreType.Single x, ScoreType.Single y -> ScoreType.Single(x + y)
        | ScoreType.Single x, ScoreType.Multiple(y, z) ->
            ScoreType.Multiple(x + y, x + z)
        | ScoreType.Multiple(x, y), ScoreType.Single z ->
            ScoreType.Multiple(x + z, y + z)
        | ScoreType.Multiple(x, y), ScoreType.Multiple(z, w) ->
            ScoreType.Multiple(x + z, y + w))

let private createHand cards =
    { Cards = cards
      Score = cards |> List.map computeScore |> sumScores }

let private dealInitialCards game =
    let dealerHand = dealCards () |> createHand

    let playerHand = dealCards () |> createHand

    { game with
        DealerHand = dealerHand
        PlayerHand = playerHand }

let private dealAnotherCard game =
    let card = dealCard ()
    let playerHand = card :: game.PlayerHand.Cards |> createHand

    { game with PlayerHand = playerHand }

let rec private dealerTurn game =
    let dealerHand = game.DealerHand.Score |> scoreValue

    if dealerHand < 17 then
        let card = dealCard ()
        let dealerHand = card :: game.DealerHand.Cards |> createHand

        { game with DealerHand = dealerHand } |> dealerTurn
    else
        game

/// Sets the player bet for the current game, if the current state is not betting,
/// the bet is invalid or the player does not have enough funds, an error is returned.
/// Otherwise the updated game state is returned.
let bet state game amount =
    let playerBalance =
        Queries.Bank.playableCharacterAccount state
        |> Queries.Bank.balanceOf state

    match game with
    | Betting ->
        if amount <= 0m<dd> || amount > 100m<dd> then
            Error InvalidBet
        else if amount > playerBalance then
            Error NotEnoughFunds
        else
            MiniGame.Blackjack.create amount
            |> dealInitialCards
            |> Playing
            |> Blackjack
            |> Situations.playingMiniGame
            |> Ok
    | _ -> Error BettingNotAllowed

type InGameActionError = NotAllowed

type HitOutcome =
    | Bust of game: BlackJackGame * Effect list
    | Continue of Effect list

let private lostExpenseEffects state game =
    let playerAccount = Queries.Bank.playableCharacterAccount state

    expense state playerAccount game.Bet
    |> Result.unwrap (* The character was validated to have enough money during the betting process. *)

let private earningEffect state amount =
    let playerAccount = Queries.Bank.playableCharacterAccount state

    income state playerAccount amount

let private withBustCheck state game =
    let playerScore = scoreValue game.PlayerHand.Score

    if playerScore > 21 then
        (* Bust, player loses their entire bet. *)
        let betExpense = lostExpenseEffects state game

        Bust(game, startGame :: betExpense)
    else
        Continue [ game |> Playing |> Blackjack |> Situations.playingMiniGame ]

/// Allows the player to hit, as long as they're still in the playing phase.
let hit state game =
    match game with
    | Playing gameState ->
        gameState |> dealAnotherCard |> withBustCheck state |> Ok
    | _ -> Error NotAllowed

type StandOutcome =
    | DealerBusted of game: BlackJackGame * Effect list
    | DealerWin of game: BlackJackGame * Effect list
    | Push of game: BlackJackGame * Effect list
    | PlayerBlackjack of game: BlackJackGame * totalEarned: Amount * Effect list
    | PlayerWin of game: BlackJackGame * Effect list

let private doStand state game =
    let game = game |> dealerTurn

    let dealerScore = game.DealerHand.Score |> scoreValue
    let playerScore = game.PlayerHand.Score |> scoreValue

    match dealerScore, playerScore with
    | dealerScore, playerScore when dealerScore = playerScore ->
        (* Push, player gets back their money (so no transactions done). *)
        Push(game, [ startGame ])
    | _, playerScore when playerScore = 21 ->
        (* Blackjack! Player gets 1.5 times the bet. *)
        let totalEarned = game.Bet * 1.5m
        let betIncome = totalEarned |> earningEffect state

        PlayerBlackjack(game, totalEarned, [ betIncome; startGame ])
    | dealerScore, playerScore when dealerScore > 21 && playerScore <= 21 ->
        (* Dealer busted, player wins. *)
        DealerBusted(game, [ game.Bet |> earningEffect state; startGame ])
    | dealerScore, playerScore when playerScore > dealerScore ->
        (* Win by higher score, player wins the bet. *)
        let playerAccount = Queries.Bank.playableCharacterAccount state
        let betIncome = game.Bet |> income state playerAccount

        PlayerWin(game, [ betIncome; startGame ])
    | _, _ ->
        (* Dealer wins, player loses all bet. *)
        let betExpense = lostExpenseEffects state game

        DealerWin(game, [ yield! betExpense; startGame ])

/// Allows the player to stay with their current hand and end their turn, which
/// calculates the dealer's hand and determines the outcome of the game.
let stand state game =
    match game with
    | Playing gameState -> doStand state gameState |> Ok
    | _ -> Error NotAllowed

/// Allows the player to leave the current mini-game as long as they're still
/// in the betting phase.
let leave state game =
    let timeEffects =
        MiniGameInGameInteraction.Leave(MiniGameId.Blackjack, game)
        |> MiniGameInteraction.InGame
        |> Interaction.MiniGame
        |> Queries.InteractionTime.timeRequired
        |> AdvanceTime.advanceDayMoment' state

    match game with
    | Betting -> Situations.freeRoam :: timeEffects |> Ok
    | _ -> Error NotAllowed

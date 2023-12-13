module Duets.Simulation.Tests.MiniGames.Blackjack

open NUnit.Framework
open FsUnit
open Test.Common
open Test.Common.Generators

open Aether
open Duets.Common
open Duets.Entities
open Duets.Entities.SituationTypes
open Duets.Simulation
open Duets.Simulation.MiniGames

let private dummyPlayingGame = MiniGame.Blackjack.create 100m<dd>

(* -------- Before the game. ------- *)

[<Test>]
let ``startGame sets the situation to betting`` () =
    Blackjack.startGame
    |> should equal (SituationChanged(PlayingMiniGame(Blackjack(Betting))))

(* -------- Betting. -------- *)

[<Test>]
let ``betting is not allowed when not in the Betting state`` () =
    Blackjack.bet dummyState (Playing dummyPlayingGame) 50m<dd>
    |> Result.unwrapError
    |> should equal Blackjack.BettingNotAllowed

[<Test>]
let ``betting is not allowed when the bet is less than 0`` () =
    Blackjack.bet dummyState Betting 0m<dd>
    |> Result.unwrapError
    |> should equal Blackjack.InvalidBet

[<Test>]
let ``betting is not allowed when the bet is more than 100`` () =
    Blackjack.bet dummyState Betting 101m<dd>
    |> Result.unwrapError
    |> should equal Blackjack.InvalidBet

[<Test>]
let ``betting is not allowed when player does not have enough balance`` () =
    let playerAccount = dummyState |> Queries.Bank.playableCharacterAccount

    let state =
        dummyState
        |> State.Bank.setBalance playerAccount (Incoming(0m<dd>, 50m<dd>))

    Blackjack.bet state Betting 80m<dd>
    |> Result.unwrapError
    |> should equal Blackjack.NotEnoughFunds

[<Test>]
let ``betting with enough balance and an allowed amount sets the situation to playing``
    ()
    =
    let effect = Blackjack.bet dummyState Betting 50m<dd> |> Result.unwrap

    match effect with
    | SituationChanged(PlayingMiniGame(Blackjack(Playing _))) -> ()
    | _ -> failwith "Unexpected effect"

(* -------- Playing. -------- *)

let private game =
    let effect = Blackjack.bet dummyState Betting 50m<dd> |> Result.unwrap

    match effect with
    | SituationChanged(PlayingMiniGame(Blackjack(Playing game))) -> game
    | _ -> failwith "Unexpected effect"

[<Test>]
let ``hitting when not playing is not allowed`` () =
    Blackjack.hit dummyState Betting
    |> Result.unwrapError
    |> should equal Blackjack.NotAllowed

[<Test>]
let ``hitting and getting less than 22 returns a Continue and updated situation effects``
    ()
    =
    let hitAllowedGame =
        { game with
            PlayerHand =
                { Cards =
                    [ { Suit = Suit.Clubs; Rank = Rank.Two }
                      { Suit = Suit.Diamonds
                        Rank = Rank.Two } ]
                  Score = ScoreType.Single 4 } }

    let result =
        Blackjack.hit dummyState (Playing hitAllowedGame) |> Result.unwrap

    match result with
    | Blackjack.Continue effects ->
        let situationEffect = effects |> List.head

        match situationEffect with
        | SituationChanged(PlayingMiniGame(Blackjack(Playing gameState))) ->
            gameState.PlayerHand.Score
            |> Blackjack.scoreValue
            |> should be (greaterThanOrEqualTo 4)
        | _ -> failwith "Unexpected effect"
    | _ -> failwith "Unexpected result"

[<Test>]
let ``hitting and getting over 21 results in busted and money is taken from the player``
    ()
    =
    (* Will deal a 10 card. *)
    use _ = changeToStaticRandom 9

    let bustGame =
        { game with
            PlayerHand =
                { Cards =
                    [ { Suit = Suit.Clubs; Rank = Rank.King }
                      { Suit = Suit.Diamonds
                        Rank = Rank.King } ]
                  Score = ScoreType.Single 10 } }

    let result = Blackjack.hit dummyState (Playing bustGame) |> Result.unwrap

    match result with
    | Blackjack.Bust(game, effects) ->
        game.PlayerHand.Score
        |> Blackjack.scoreValue
        |> should be (greaterThanOrEqualTo 22)

        effects |> List.head |> should be (ofCase (<@ SituationChanged @>))
        effects |> List.item 1 |> should be (ofCase (<@ MoneyTransferred @>))
    | _ -> failwith "Unexpected result"

[<Test>]
let ``standing when both player and dealer have the same hand value results in push``
    ()
    =
    let pushGame =
        { game with
            PlayerHand =
                { Cards =
                    [ { Suit = Suit.Clubs; Rank = Rank.King }
                      { Suit = Suit.Diamonds
                        Rank = Rank.King } ]
                  Score = ScoreType.Single 20 }
            DealerHand =
                { Cards =
                    [ { Suit = Suit.Clubs; Rank = Rank.King }
                      { Suit = Suit.Diamonds
                        Rank = Rank.King } ]
                  Score = ScoreType.Single 20 } }

    let result = Blackjack.stand dummyState (Playing pushGame) |> Result.unwrap

    match result with
    | Blackjack.Push(game, effects) ->
        game.PlayerHand.Score |> Blackjack.scoreValue |> should be (equal 20)

        game.DealerHand.Score |> Blackjack.scoreValue |> should be (equal 20)

        effects |> List.head |> should be (ofCase (<@ SituationChanged @>))
    | _ -> failwith "Unexpected result"

[<Test>]
let ``standing when both player and dealer have 21 results in push`` () =
    let pushGame =
        { game with
            PlayerHand =
                { Cards =
                    [ { Suit = Suit.Clubs; Rank = Rank.King }
                      { Suit = Suit.Diamonds
                        Rank = Rank.Ace } ]
                  Score = ScoreType.Multiple(11, 21) }
            DealerHand =
                { Cards =
                    [ { Suit = Suit.Clubs; Rank = Rank.King }
                      { Suit = Suit.Diamonds
                        Rank = Rank.Ace } ]
                  Score = ScoreType.Multiple(11, 21) } }

    let result = Blackjack.stand dummyState (Playing pushGame) |> Result.unwrap

    match result with
    | Blackjack.Push(game, effects) ->
        game.PlayerHand.Score |> Blackjack.scoreValue |> should be (equal 21)

        game.DealerHand.Score |> Blackjack.scoreValue |> should be (equal 21)

        effects |> List.head |> should be (ofCase (<@ SituationChanged @>))
    | _ -> failwith "Unexpected result"

[<Test>]
let ``standing when player has 21 and dealer does not results in blackjack for player``
    ()
    =
    let blackjackGame =
        { PlayerHand =
            { Cards =
                [ { Suit = Suit.Clubs; Rank = Rank.King }
                  { Suit = Suit.Diamonds
                    Rank = Rank.Ace } ]
              Score = ScoreType.Multiple(11, 21) }
          DealerHand =
            { Cards =
                [ { Suit = Suit.Clubs; Rank = Rank.King }
                  { Suit = Suit.Diamonds
                    Rank = Rank.Seven } ]
              Score = ScoreType.Single 17 }
          Bet = 50m<dd> }

    let result =
        Blackjack.stand dummyState (Playing blackjackGame) |> Result.unwrap

    match result with
    | Blackjack.PlayerBlackjack(game, amount, effects) ->
        game.PlayerHand.Score |> Blackjack.scoreValue |> should be (equal 21)
        game.DealerHand.Score |> Blackjack.scoreValue |> should be (equal 17)

        amount |> should equal 75m<dd>

        effects |> List.head |> should be (ofCase (<@ MoneyEarned @>))
        effects |> List.item 1 |> should be (ofCase (<@ SituationChanged @>))
    | _ -> failwith "Unexpected result"

[<Test>]
let ``standing when player has less than 21 and dealer busts results in win for player``
    ()
    =
    let dealerBustGame =
        { PlayerHand =
            { Cards =
                [ { Suit = Suit.Clubs; Rank = Rank.King }
                  { Suit = Suit.Diamonds
                    Rank = Rank.Seven } ]
              Score = ScoreType.Single 17 }
          DealerHand =
            { Cards =
                [ { Suit = Suit.Clubs; Rank = Rank.King }
                  { Suit = Suit.Diamonds
                    Rank = Rank.King }
                  { Suit = Suit.Hearts
                    Rank = Rank.Three } ]
              Score = ScoreType.Single 23 }
          Bet = 50m<dd> }

    let result =
        Blackjack.stand dummyState (Playing dealerBustGame) |> Result.unwrap

    match result with
    | Blackjack.DealerBusted(game, effects) ->
        game.PlayerHand.Score |> Blackjack.scoreValue |> should be (equal 17)
        game.DealerHand.Score |> Blackjack.scoreValue |> should be (equal 23)

        effects |> List.head |> should be (ofCase (<@ MoneyEarned @>))
        effects |> List.item 1 |> should be (ofCase (<@ SituationChanged @>))
    | _ -> failwith "Unexpected result"

[<Test>]
let ``standing when player has more than dealer results in win for player`` () =
    let winGame =
        { PlayerHand =
            { Cards =
                [ { Suit = Suit.Clubs; Rank = Rank.King }
                  { Suit = Suit.Diamonds
                    Rank = Rank.Nine } ]
              Score = ScoreType.Single 19 }
          DealerHand =
            { Cards =
                [ { Suit = Suit.Clubs; Rank = Rank.King }
                  { Suit = Suit.Diamonds
                    Rank = Rank.Seven } ]
              Score = ScoreType.Single 17 }
          Bet = 50m<dd> }

    let result = Blackjack.stand dummyState (Playing winGame) |> Result.unwrap

    match result with
    | Blackjack.PlayerWin(game, effects) ->
        game.PlayerHand.Score |> Blackjack.scoreValue |> should be (equal 19)
        game.DealerHand.Score |> Blackjack.scoreValue |> should be (equal 17)

        effects |> List.head |> should be (ofCase (<@ MoneyEarned @>))
        effects |> List.item 1 |> should be (ofCase (<@ SituationChanged @>))
    | _ -> failwith "Unexpected result"

[<Test>]
let ``standing when player has less than dealer results in loss for player``
    ()
    =
    let lossGame =
        { PlayerHand =
            { Cards =
                [ { Suit = Suit.Clubs; Rank = Rank.King }
                  { Suit = Suit.Diamonds
                    Rank = Rank.Seven } ]
              Score = ScoreType.Single 17 }
          DealerHand =
            { Cards =
                [ { Suit = Suit.Clubs; Rank = Rank.King }
                  { Suit = Suit.Diamonds
                    Rank = Rank.King } ]
              Score = ScoreType.Single 20 }
          Bet = 50m<dd> }

    let result = Blackjack.stand dummyState (Playing lossGame) |> Result.unwrap

    match result with
    | Blackjack.DealerWin(game, effects) ->
        game.PlayerHand.Score |> Blackjack.scoreValue |> should be (equal 17)
        game.DealerHand.Score |> Blackjack.scoreValue |> should be (equal 20)

        effects |> List.head |> should be (ofCase (<@ MoneyTransferred @>))
        effects |> List.item 1 |> should be (ofCase (<@ SituationChanged @>))
    | _ -> failwith "Unexpected result"

[<Test>]
let ``standing when the dealer has less than 17 triggers a turn for the dealer``
    ()
    =
    use _ = changeToStaticRandom 2 (* Will deal a Three. *)

    let nonFinishedGame =
        { PlayerHand =
            { Cards =
                [ { Suit = Suit.Clubs; Rank = Rank.King }
                  { Suit = Suit.Diamonds
                    Rank = Rank.Seven } ]
              Score = ScoreType.Single 17 }
          DealerHand =
            { Cards =
                [ { Suit = Suit.Clubs; Rank = Rank.King }
                  { Suit = Suit.Diamonds
                    Rank = Rank.Six } ]
              Score = ScoreType.Single 16 }
          Bet = 50m<dd> }

    let result =
        Blackjack.stand dummyState (Playing nonFinishedGame) |> Result.unwrap

    match result with
    | Blackjack.DealerWin(game, _) ->
        game.PlayerHand.Score |> Blackjack.scoreValue |> should be (equal 17)
        game.DealerHand.Score |> Blackjack.scoreValue |> should be (equal 19)
    | _ -> failwith "Unexpected result"

(* -------- After the game. -------- *)

[<Test>]
let ``leaving sets the situation back to free roam and increase time`` () =
    let effects = Blackjack.leave dummyState Betting |> Result.unwrap

    effects |> List.head |> should be (ofCase (<@ SituationChanged @>))
    effects |> List.item 1 |> should be (ofCase (<@ TimeAdvanced @>))

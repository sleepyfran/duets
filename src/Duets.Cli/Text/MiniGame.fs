module Duets.Cli.Text.MiniGame

open Duets.Entities

let score sc =
    match sc with
    | ScoreType.Single x -> $"{x}"
    | ScoreType.Multiple(x, y) -> $"{x} or {y}"

let card c =
    let suit =
        match c.Suit with
        | Clubs -> Emoji.clubs
        | Diamonds -> Emoji.diamonds
        | Hearts -> Emoji.hearts
        | Spades -> Emoji.spades

    let rank =
        match c.Rank with
        | Ace -> "Ace"
        | Two -> "2"
        | Three -> "3"
        | Four -> "4"
        | Five -> "5"
        | Six -> "6"
        | Seven -> "7"
        | Eight -> "8"
        | Nine -> "9"
        | Ten -> "10"
        | Jack -> "Jack"
        | Queen -> "Queen"
        | King -> "King"

    $"{suit} {rank}"

let private cards cs =
    $"""{cs |> List.map card |> String.concat " "}"""

let dealerHiddenHand game =
    $"{game.DealerHand.Cards |> List.head |> card} [[?]]"

let dealerFullHand game = game.DealerHand.Cards |> cards

let playerHand game = game.PlayerHand.Cards |> cards

let private blackjackActionPrompt miniGameState =
    match miniGameState with
    | Betting ->
        $"{Emoji.cards} Betting for the next blackjack round..." |> Styles.faded
    | Playing game ->
        $"""{Styles.faded "Dealer: "} {game |> dealerHiddenHand}
{Styles.faded "   You: "} {game |> playerHand}
{Styles.faded " Value: "} {game.PlayerHand.Score |> score}
{Styles.faded "   Bet: "} {game.Bet |> Styles.money}
{Styles.action "Would you like to hit or stand?"}"""

let actionPrompt date dayMoment miniGameState =
    let miniGamePrompt =
        match miniGameState with
        | Blackjack state -> blackjackActionPrompt state

    $"{Generic.timeBar date dayMoment}
{miniGamePrompt}"

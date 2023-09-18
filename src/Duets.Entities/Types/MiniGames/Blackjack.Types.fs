namespace Duets.Entities

[<AutoOpen>]
module BlackjackTypes =
    /// Defines the type of score a player or dealer has. This is needed
    /// because aces can be worth 1 or 11.
    [<RequireQualifiedAccess>]
    type ScoreType =
        | Single of int
        | Multiple of int * int

    /// Defines the hand of a player or dealer.
    type Hand = { Cards: Card list; Score: ScoreType }

    /// Defines the current state of a game of blackjack.
    type BlackJackGame =
        { DealerHand: Hand
          PlayerHand: Hand
          Bet: Amount }

    /// Defines the current state of a game of blackjack.
    type BlackJackGameState =
        | Betting
        | Playing of BlackJackGame

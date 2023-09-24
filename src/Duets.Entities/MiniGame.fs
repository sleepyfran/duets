module Duets.Entities.MiniGame

open Duets.Common

let allSuits = Union.allCasesOf<Suit> ()

let allRanks = Union.allCasesOf<Rank> ()

/// Contains all the possible cards in a deck.
let allCards =
    List.allPairs allSuits allRanks
    |> List.map (fun (suit, rank) -> { Suit = suit; Rank = rank })

module Blackjack =
    /// Creates a new blackjack game given a bet.
    let create bet =
        { DealerHand =
            { Cards = []
              Score = ScoreType.Single 0 }
          PlayerHand =
            { Cards = []
              Score = ScoreType.Single 0 }
          Bet = bet }

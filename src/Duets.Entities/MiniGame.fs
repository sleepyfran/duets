module Duets.Entities.MiniGame

open FSharp.Reflection

let allSuits =
    FSharpType.GetUnionCases typeof<Suit>
    |> Array.map (fun uc -> FSharpValue.MakeUnion(uc, [||]) :?> Suit)
    |> List.ofArray

let allRanks =
    FSharpType.GetUnionCases typeof<Rank>
    |> Array.map (fun uc -> FSharpValue.MakeUnion(uc, [||]) :?> Rank)
    |> List.ofArray

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

namespace Duets.Entities

[<AutoOpen>]
module MiniGameSharedTypes =
    /// Defines the suits of a standard deck of cards.
    type Suit =
        | Clubs
        | Diamonds
        | Hearts
        | Spades

    /// Defines the ranks of a standard deck of cards.
    type Rank =
        | Ace
        | Two
        | Three
        | Four
        | Five
        | Six
        | Seven
        | Eight
        | Nine
        | Ten
        | Jack
        | Queen
        | King

    /// Defines a card in a standard deck of cards.
    type Card = { Suit: Suit; Rank: Rank }

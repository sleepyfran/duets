namespace Duets.Entities

[<AutoOpen>]
module MiniGameTypes =
    /// Defines the types of mini-games that the game supports.
    [<RequireQualifiedAccess>]
    type MiniGameId = Blackjack

    /// Defines the types of mini-games that the game supports, with their
    /// associated state.
    type MiniGameState = Blackjack of BlackJackGameState

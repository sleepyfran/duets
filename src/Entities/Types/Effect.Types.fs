namespace Entities

[<AutoOpen>]
module EffectTypes =
    /// Defines an effect that happened after an action in the game. For example
    /// calling composeSong will create a `SongComposed` effect.
    type Effect =
        | GameCreated of State
        | TimeAdvanced of Date
        | SongStarted of Band * UnfinishedSongWithQualities
        | SongImproved of Band * Diff<UnfinishedSongWithQualities>
        | SongFinished of Band * FinishedSongWithQuality
        | SongDiscarded of Band * UnfinishedSongWithQualities
        | SongPracticed of Band * FinishedSongWithQuality
        | MemberHired of Band * CurrentMember * SkillWithLevel list
        | MemberFired of Band * CurrentMember * PastMember
        | SkillImproved of Character * Diff<SkillWithLevel>
        | MoneyTransferred of BankAccountHolder * BankTransaction
        | MoneyEarned of BankAccountHolder * BankTransaction
        | AlbumRecorded of Band * UnreleasedAlbum
        | AlbumRenamed of Band * UnreleasedAlbum
        | AlbumReleased of Band * ReleasedAlbum
        | AlbumReleasedUpdate of Band * ReleasedAlbum
        | GenreMarketsUpdated of GenreMarketByGenre
        | WorldMoveTo of Coordinates

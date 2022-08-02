namespace Entities

open Entities

[<AutoOpen>]
module EffectTypes =
    /// Defines an effect that happened after an action in the game. For example
    /// calling composeSong will create a `SongComposed` effect.
    type Effect =
        | AlbumRecorded of Band * UnreleasedAlbum
        | AlbumReleased of Band * ReleasedAlbum
        | AlbumReleasedUpdate of Band * ReleasedAlbum
        | AlbumRenamed of Band * UnreleasedAlbum
        | CharacterAttributeChanged of
            character: Character *
            attribute: CharacterAttribute *
            diff: Diff<CharacterAttributeAmount>
        | CharacterHealthDepleted of Character
        | CharacterHospitalized of Character * WorldCoordinates
        | ConcertScheduled of Band * ScheduledConcert
        | ConcertFinished of Band * PastConcert
        | ConcertUpdated of Band * ScheduledConcert
        | ConcertCancelled of Band * PastConcert
        | GameCreated of State
        | GenreMarketsUpdated of GenreMarketByGenre
        | InventoryItemAdded of Item
        | InventoryItemRemoved of Item
        | MemberHired of Band * Character * CurrentMember * SkillWithLevel list
        | MemberFired of Band * CurrentMember * PastMember
        | MoneyEarned of BankAccountHolder * BankTransaction
        | MoneyTransferred of BankAccountHolder * BankTransaction
        | SkillImproved of Character * Diff<SkillWithLevel>
        | SongStarted of Band * UnfinishedSongWithQualities
        | SongImproved of Band * Diff<UnfinishedSongWithQualities>
        | SongFinished of Band * FinishedSongWithQuality
        | SongDiscarded of Band * UnfinishedSongWithQualities
        | SongPracticed of Band * FinishedSongWithQuality
        | SituationChanged of Situation
        | TimeAdvanced of Date
        | WorldMoveTo of WorldCoordinates
        | Wait of int

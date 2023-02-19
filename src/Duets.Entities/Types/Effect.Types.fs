namespace Duets.Entities

open Duets.Entities.SituationTypes

[<AutoOpen>]
module EffectTypes =
    /// Defines an effect that happened after an action in the game. For example
    /// calling composeSong will create a `SongComposed` effect.
    type Effect =
        | AlbumStarted of Band * UnreleasedAlbum
        | AlbumReleased of Band * ReleasedAlbum
        | AlbumReleasedUpdate of Band * ReleasedAlbum
        | AlbumUpdated of Band * UnreleasedAlbum
        | BandFansChanged of Band * Diff<Fans>
        | CharacterAttributeChanged of
            character: CharacterId *
            attribute: CharacterAttribute *
            diff: Diff<CharacterAttributeAmount>
        | CharacterHealthDepleted of CharacterId
        | CharacterHospitalized of CharacterId * WorldCoordinates
        | CareerAccept of CharacterId * Job
        | CareerLeave of CharacterId * Job
        | ConcertScheduled of Band * ScheduledConcert
        | ConcertFinished of band: Band * concert: PastConcert * income: Amount
        | ConcertUpdated of Band * ScheduledConcert
        | ConcertCancelled of Band * PastConcert
        | FlightBooked of Flight
        | FlightUpdated of Flight
        | GameCreated of State
        | GenreMarketsUpdated of GenreMarketByGenre
        | ItemAddedToInventory of Item
        | ItemRemovedFromInventory of Item
        | ItemRemovedFromWorld of WorldCoordinates * Item
        | MemberHired of Band * Character * CurrentMember * SkillWithLevel list
        | MemberFired of Band * CurrentMember * PastMember
        | MoneyEarned of BankAccountHolder * BankTransaction
        | MoneyTransferred of BankAccountHolder * BankTransaction
        | NotificationEventHappeningSoon of CalendarEventType
        | RentalExpired of Place
        | SkillImproved of Character * Diff<SkillWithLevel>
        | SongStarted of Band * UnfinishedSongWithQualities
        | SongImproved of Band * Diff<UnfinishedSongWithQualities>
        | SongFinished of Band * FinishedSongWithQuality
        | SongDiscarded of Band * UnfinishedSongWithQualities
        | SongPracticed of Band * FinishedSongWithQuality
        | SituationChanged of Situation
        | PlaceClosed of Place
        | TimeAdvanced of Date
        | WorldMoveTo of WorldCoordinates
        | Wait of int<dayMoments>
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
        | AlbumReviewsReceived of Band * ReleasedAlbum
        | AlbumUpdated of Band * UnreleasedAlbum
        | BalanceUpdated of BankAccountHolder * Diff<Amount>
        | BandFansChanged of Band * Diff<Fans>
        | BandSwitchedGenre of Band * Diff<Genre>
        | CareerAccept of CharacterId * Job
        | CareerLeave of CharacterId * Job
        | CareerPromoted of Job * salary: Amount
        | CareerShiftPerformed of Job * payment: Amount
        | CharacterAttributeChanged of
            character: CharacterId *
            attribute: CharacterAttribute *
            diff: Diff<CharacterAttributeAmount>
        | CharacterHealthDepleted of CharacterId
        | CharacterHospitalized of CharacterId * PlaceCoordinates
        | CharacterMoodletsChanged of
            character: CharacterId *
            moodlets: Diff<CharacterMoodlets>
        | ConcertScheduled of Band * ScheduledConcert
        | ConcertFinished of band: Band * concert: PastConcert * income: Amount
        | ConcertUpdated of Band * ScheduledConcert
        | ConcertCancelled of Band * PastConcert
        | FlightBooked of Flight
        | FlightUpdated of Flight
        | GameCreated of State
        | GenreMarketsUpdated of GenreMarketByGenre
        | ItemAddedToInventory of Item
        | ItemChangedInInventory of Diff<Item>
        | ItemChangedInWorld of RoomCoordinates * Diff<Item>
        | ItemRemovedFromInventory of Item
        | ItemRemovedFromWorld of RoomCoordinates * Item
        | MemberHired of Band * Character * CurrentMember * SkillWithLevel list
        | MemberFired of Band * CurrentMember * PastMember
        | MoneyEarned of BankAccountHolder * BankTransaction
        | MoneyTransferred of BankAccountHolder * BankTransaction
        | Notification of Notification
        | RelationshipChanged of
            npcId: Character *
            cityId: CityId *
            relationship: Relationship option
        | RentalAdded of Rental
        | RentalKickedOut of Rental
        | RentalExpired of Rental
        | RentalUpdated of Rental
        | SituationChanged of Situation
        | SkillImproved of Character * Diff<SkillWithLevel>
        | SocialNetworkAccountCreated of SocialNetworkKey * SocialNetworkAccount
        | SocialNetworkAccountFollowersChanged of
            SocialNetworkKey *
            SocialNetworkAccountId *
            Diff<int>
        | SocialNetworkCurrentAccountChanged of
            SocialNetworkKey *
            SocialNetworkAccountId
        | SocialNetworkPost of SocialNetworkKey * SocialNetworkPost
        | SocialNetworkPostReposted of
            SocialNetworkKey *
            SocialNetworkPost *
            int
        | SongStarted of Band * Unfinished<Song>
        | SongImproved of Band * Diff<Unfinished<Song>>
        | SongFinished of Band * Finished<Song> * finishDate: Date
        | SongDiscarded of Band * Unfinished<Song>
        | SongPracticed of Band * Finished<Song>
        | PlaceClosed of Place
        | PlayResult of PlayResult
        | TimeAdvanced of Date
        /// Moves the player to a new room inside the current place.
        | WorldEnter of Diff<RoomCoordinates>
        /// Moves the player to a different place in the current city or a
        /// different one.
        | WorldMoveTo of Diff<RoomCoordinates>
        | WorldPeopleInCurrentRoomChanged of Character list
        | Wait of int<dayMoments>

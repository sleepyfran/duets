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
        | AlbumSongAdded of Band * UnreleasedAlbum * Recorded<Song>
        | AlbumUpdated of Band * UnreleasedAlbum
        | Ate of item: Item * food: EdibleItem
        | BalanceUpdated of BankAccountHolder * Diff<Amount>
        | BandFansChanged of Band * Diff<Fans>
        | BandSwitchedGenre of Band * Diff<Genre>
        | BookRead of Item * Book
        | CareerAccept of CharacterId * Job
        | CareerLeave of CharacterId * Job
        | CareerPromoted of Job * salary: Amount
        | CareerShiftPerformed of
            Job *
            shiftDuration: int<dayMoments> *
            payment: Amount
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
        | ConcertSoundcheckPerformed
        | Drank of item: Item * drink: DrinkableItem
        | Exercised of Item
        | FlightBooked of Flight
        | FlightUpdated of Flight
        | FlightLanded of Flight
        | GameCreated of State
        | GamePlayed of PlayResult
        | GenreMarketsUpdated of GenreMarketByGenre
        | ItemAddedToCharacterInventory of Item
        | ItemChangedInCharacterInventory of Diff<Item>
        | ItemRemovedFromCharacterInventory of Item
        | ItemAddedToBandInventory of Band * Item * int<quantity>
        | ItemAddedToWorld of RoomCoordinates * Item
        | ItemChangedInWorld of RoomCoordinates * Diff<Item>
        | ItemRemovedFromWorld of RoomCoordinates * Item
        | MerchPriceSet of band: Band * merchItem: Item * price: Amount
        | MerchSold of band: Band * (Item * int<quantity>) list * income: Amount
        | MerchStandSetup
        | MemberHired of Band * Character * CurrentMember * SkillWithLevel list
        | MemberFired of Band * CurrentMember * PastMember
        | MiniGamePlayed of MiniGameId
        | MoneyEarned of BankAccountHolder * BankTransaction
        | MoneyTransferred of BankAccountHolder * BankTransaction
        | NotificationScheduled of Date * DayMoment * Notification
        | NotificationShown of Notification
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
        | TimeAdvanced of Date
        | TurnTimeUpdated of int<minute>
        | WatchedTv of Item
        /// Moves the player to a new room inside the current place.
        | WorldEnterRoom of Diff<RoomCoordinates>
        /// Moves the player to a different place in the current city or a
        /// different one.
        | WorldMoveToPlace of Diff<RoomCoordinates>
        | WorldPeopleInCurrentRoomChanged of Character list
        | Wait of int<dayMoments>

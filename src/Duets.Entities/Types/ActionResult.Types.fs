namespace Duets.Entities

open Duets.Entities.SituationTypes

[<AutoOpen>]
module EffectTypes =
    /// Defines an effect that happened after an action in the game. For example
    /// calling composeSong will create a `SongComposed` effect.
    type Effect =
        | AlbumSongAdded of Band * UnreleasedAlbum * Finished<Song>
        | AlbumStarted of Band * UnreleasedAlbum
        | AlbumReleased of Band * ReleasedAlbum
        | AlbumReleasedUpdate of Band * ReleasedAlbum
        | AlbumReviewsReceived of Band * ReleasedAlbum
        | AlbumRenamed of Band * UnreleasedAlbum
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
        | ItemAddedToCharacterInventory of Item
        | ItemChangedInCharacterInventory of Diff<Item>
        | ItemRemovedFromCharacterInventory of Item
        | ItemRemovedBySecurity of
            Item (* TODO: Consider adding a context to the effect above instead. *)
        | ItemAddedToBandInventory of Item * int<quantity>
        | ItemAddedToWorld of RoomCoordinates * Item
        | ItemChangedInWorld of RoomCoordinates * Diff<Item>
        | ItemRemovedFromWorld of RoomCoordinates * Item
        | MerchPriceSet of band: Band * merchItem: Item * price: Amount
        | MerchSold of band: Band * (Item * int<quantity>) list * income: Amount
        | MemberHired of Band * Character * CurrentMember * SkillWithLevel list
        | MemberFired of Band * CurrentMember * PastMember
        | MoneyEarned of BankAccountHolder * BankTransaction
        | MoneyTransferred of BankAccountHolder * BankTransaction
        | NotificationScheduled of Date * DayMoment * Notification
        | NotificationShown of Notification
        | PlaceClosed of Place
        | PlaneBoarded of flight: Flight * flightTime: int<minute>
        | PlayedGame of PlayResult
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
        | TimeAdvanced of Date
        /// Moves the player to a new room inside the current place.
        | WorldEnterRoom of Diff<RoomCoordinates>
        /// Moves the player to a different place in the current city or a
        /// different one.
        | WorldMoveToPlace of Diff<RoomCoordinates>
        | WorldPeopleInCurrentRoomChanged of Character list
        | Wait of int<dayMoments>

[<AutoOpen>]
module ErrorTypes =
    /// Defines all the possible errors that can happen while executing an action.
    type ActionError =
        | CannotFirePlayableCharacter
        | NotEnoughFundsToRecordAlbum of Amount
        | SongAlreadyImprovedToMax of Finished<Song>

[<AutoOpen>]
module ActionResultTypes =
    /// Defines the result of an action, which can either be a list of effects
    /// that happened during the action and the updated state, or an error.
    type ActionResult = Result<Effect list * State, ActionError>

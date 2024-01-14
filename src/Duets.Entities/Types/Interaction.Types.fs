namespace Duets.Entities

open Duets.Entities

[<AutoOpen>]
module InteractionTypes =
    [<RequireQualifiedAccess>]
    type AirportInteraction =
        /// Allows the character to board a flight they've previously booked.
        | BoardAirplane of Flight
        /// Allows the character to enter the boarding gate.
        | PassSecurity
        /// Allows the character to wait until the flight ends.
        | WaitUntilLanding of Flight

    [<RequireQualifiedAccess>]
    type CareerInteraction =
        /// Starts a work shift in the given job.
        | Work of job: Job

    /// Interactions that can be done while on a concert.
    [<RequireQualifiedAccess>]
    type ConcertInteraction =
        /// Sets up a merch stand on the venue so that the band can sell
        /// merchandise to the crowd.
        | SetupMerchStand of
            checklist: ConcertPreparationChecklist *
            itemsWithoutPrice: Item list
        /// Performs a sound check, which gives additional points to the overall
        /// concert score.
        | PerformSoundCheck of checklist: ConcertPreparationChecklist
        /// Starts a scheduled concert.
        | StartConcert of ScheduledConcert
        /// Makes the character adjust the drum setting. Does nothing.
        | AdjustDrums of OngoingConcert
        /// Performs a bass solo, which gives points based on the player's
        /// bass skills.
        | BassSolo of OngoingConcert
        /// Dedicates a song to a certain someone.
        | DedicateSong of OngoingConcert
        /// Allows to play more songs after stepping out of the stage.
        | DoEncore of OngoingConcert
        /// Performs a drum solo, which gives points based on the player's
        /// drumming skills.
        | DrumSolo of OngoingConcert
        /// Moves back to the backstage, potentially ending the concert.
        | GetOffStage of OngoingConcert
        /// Allows to give a speech to the crowd.
        | GiveSpeech of OngoingConcert
        /// Hallo!
        | GreetAudience of OngoingConcert
        /// Performs a guitar solo, which gives points based on the player's
        /// guitar skills.
        | GuitarSolo of OngoingConcert
        /// Puts the player facing to the band and giving the back to the crowd.
        | FaceBand of OngoingConcert
        /// Puts the player facing the crowd again.
        | FaceCrowd of OngoingConcert
        /// Finishes the concert, which computes the final score and disallows
        /// the band from entering the stage again.
        | FinishConcert of OngoingConcert
        /// Allows the character to interact with the crowd by making them
        /// sing a few lines. Gives points depending on the player's vocal skills
        /// and the band fame.
        | MakeCrowdSing of OngoingConcert
        /// Allows to play a song.
        | PlaySong of OngoingConcert
        /// Makes the character put the microphone back on the stand.
        | PutMicOnStand of OngoingConcert
        /// Makes the player do that cool drummer trick. Gives a little point increase.
        | SpinDrumsticks of OngoingConcert
        /// Makes the character pick up the microphone.
        | TakeMic of OngoingConcert
        /// Tunes the player's instrument, which gives a tiny point increase.
        | TuneInstrument of OngoingConcert

    /// Interactions that can be done inside of a gym.
    [<RequireQualifiedAccess>]
    type GymInteraction = PayEntrance of price: Amount

    /// Interactions related to items in the world or the character's inventory.
    [<RequireQualifiedAccess>]
    type ItemInteraction =
        | Cook of PurchasableItem list
        | Drink
        | Eat
        | Exercise
        | Open
        | Put
        | Play
        | Read
        | Sleep
        | Watch

    /// Interactions related to moving around the world.
    [<RequireQualifiedAccess>]
    type FreeRoamInteraction =
        /// Allows the player to see the current time and how many day moments
        /// are left in the day.
        | Clock of (DayMoment * CalendarEventType list) list
        /// Allows the player to see what they are currently carrying.
        | Inventory of inventory: Item list
        /// Allows the user to look around and see which objects are available.
        | Look of
            items: Item list *
            knownCharacters: Character list *
            unknownCharacters: Character list
        /// Allows the player to travel around the current city.
        | Map
        /// Allows movement into the specified direction.
        | Move of direction: Direction * roomId: NodeId
        /// Allows the character to use the phone.
        | Phone
        /// Allows waiting.
        | Wait

    /// Interactions that can be done in a merchandise workshop.
    [<RequireQualifiedAccess>]
    type MerchandiseWorkshopInteraction =
        /// Allows the player to order new merchandise.
        | OrderMerchandise of available: MerchandiseItem list
        /// Allows the player to list the merchandise that they've ordered.
        | ListOrderedMerchandise of (Date * DeliverableItem) list
        /// Allows the player to pick up the merchandise that they've ordered.
        | PickUpMerchandise of Item list

    /// Interactions that can be done when playing blackjack.
    [<RequireQualifiedAccess>]
    type MiniGameInGameInteraction =
        /// Allows the player to bet for some game.
        | Bet of BlackJackGameState
        /// Allows the player to hit on Blackjack.
        | Hit of BlackJackGameState
        /// Allows the player to stand on Blackjack.
        | Stand of BlackJackGameState
        /// Allows the player to leave the game.
        | Leave of MiniGameId * BlackJackGameState

    /// Interactions related to a mini-game.
    [<RequireQualifiedAccess>]
    type MiniGameInteraction =
        /// Allows the player to perform an action in the mini-game.
        | InGame of MiniGameInGameInteraction
        /// Allows the player to start a mini-game.
        | StartGame of MiniGameId

    /// Interactions that can be done when the character is on the rehearsal room.
    [<RequireQualifiedAccess>]
    type RehearsalInteraction =
        /// Allows to list all the merchandise that the band has.
        | BandInventory of inventory: (Item * int<quantity>) list
        /// Allows to compose new songs.
        | ComposeNewSong
        /// Allows to discard an unfinished song.
        | DiscardSong of songs: Unfinished<Song> list
        /// Allows to finish an unfinished song.
        | FinishSong of songs: Unfinished<Song> list
        /// Allows to fire a member of the band.
        | FireMember of members: CurrentMember list
        /// Allows to hire a new member for the band.
        | HireMember
        /// Allows to improve an unfinished song.
        | ImproveSong of songs: Unfinished<Song> list
        /// Allows to list the members of the band.
        | ListMembers of
            members: CurrentMember list *
            pastMembers: PastMember list
        /// Allows to list all unfinished and finished songs that the band has composed.
        | ListSongs of
            unfinished: Unfinished<Song> list *
            finished: Finished<Song> list
        /// Allows to practice a finished song.
        | PracticeSong of songs: Finished<Song> list
        /// Allows the band to switch to another genre.
        | SwitchGenre of GenrePopularity list

    /// Interactions that can be performed in a bar.
    [<RequireQualifiedAccess>]
    type ShopInteraction =
        /// Allows the character to buy a certain item from a selection of
        /// available items in a shop.
        | Buy of PurchasableItem list
        /// Allows the character to order a certain item from a selection of
        /// available items in a shop.
        | Order of PurchasableItem list
        /// Allows the character to peek at the available items on the shop.
        | SeeMenu of PurchasableItem list

    /// Interactions that can be performed with an NPC.
    [<RequireQualifiedAccess>]
    type SocialInteraction =
        /// Allows the player to start a conversation with an NPC.
        | StartConversation of
            knownNpcs: Character list *
            unknownNpcs: Character list
        /// Allows the player to stop a conversation with an NPC.
        | StopConversation
        /// Allows the player to perform a social action on the NPC.
        | Action of state: SocializingState * action: SocialActionKind

    /// Interactions that can only be performed inside of a studio.
    [<RequireQualifiedAccess>]
    type StudioInteraction =
        /// Allows to create and record a new album.
        | CreateAlbum of studio: Studio * songs: Finished<Song> list
        /// Allows to record another song for a previously created album.
        | AddSongToAlbum of
            studio: Studio *
            unreleasedAlbums: UnreleasedAlbum list *
            songs: Finished<Song> list
        /// Allows to edit the name of a previously created album.
        | EditAlbumName of albums: UnreleasedAlbum list
        /// Allows to list all the unreleased albums.
        | ListUnreleasedAlbums of albums: UnreleasedAlbum list
        /// Allows to release a previously recorded album.
        | ReleaseAlbum of albums: UnreleasedAlbum list

    /// Defines all interactions that can be performed in the game. These
    /// interactions are passed back into the CLI layer to actually execute the
    /// flow. Interactions should have all the necessary information for the
    /// CLI to execute a certain action. For example: Move should include the
    /// direction towards which the movement is possible.
    [<RequireQualifiedAccess>]
    type Interaction =
        | Airport of AirportInteraction
        | Career of CareerInteraction
        | Concert of ConcertInteraction
        | Gym of GymInteraction
        | FreeRoam of FreeRoamInteraction
        | Item of ItemInteraction
        | MiniGame of MiniGameInteraction
        | MerchandiseWorkshop of MerchandiseWorkshopInteraction
        | Rehearsal of RehearsalInteraction
        | Shop of ShopInteraction
        | Social of SocialInteraction
        | Studio of StudioInteraction

    /// Defines all possible reasons why an interaction can be disabled.
    [<RequireQualifiedAccess>]
    type InteractionDisabledReason =
        | NotEnoughAttribute of
            attribute: CharacterAttribute *
            amount: CharacterAttributeAmount

    /// Defines the state of an interaction, which can be enabled or disabled
    /// for a specific reason.
    [<RequireQualifiedAccess>]
    type InteractionState =
        | Enabled
        | Disabled of InteractionDisabledReason

    /// Wraps an interaction and adds whether it's enabled or not and the amount
    /// of time that it'll take to perform it.
    type InteractionWithMetadata =
        { Interaction: Interaction
          State: InteractionState
          TimeAdvance: int<dayMoments> }

    /// Defines a simple win/lose result for a non-interactive game.
    [<RequireQualifiedAccess>]
    type SimpleResult =
        | Win
        | Lose

    /// Defines which result the game that was chosen by the character had.
    /// These are not to be confused with mini-games, which are fully interactive
    /// and are defined separetely. These are just "rabbit hole" games that don't
    /// require any interaction from the player and just give some rewards/penalties
    /// based on skills or luck.
    [<RequireQualifiedAccess>]
    type PlayResult =
        | Darts of SimpleResult
        | Pool of SimpleResult
        | VideoGame

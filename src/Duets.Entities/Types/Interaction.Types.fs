namespace Duets.Entities

open Duets.Entities

[<AutoOpen>]
module InteractionTypes =
    [<RequireQualifiedAccess>]
    type AirportInteraction =
        /// Allows the character to board a flight they've previously booked.
        | BoardAirplane of Flight
        /// Allows the character to wait until the flight ends.
        | WaitUntilLanding of Flight

    [<RequireQualifiedAccess>]
    type CareerInteraction =
        /// Starts a work shift in the given job.
        | Work of job: Job

    /// Interactions that can be done while on a concert.
    [<RequireQualifiedAccess>]
    type ConcertInteraction =
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

    /// Interactions that can be done to consume an item in particular.
    [<RequireQualifiedAccess>]
    type ConsumableItemInteraction =
        | Drink
        | Eat

    /// Interactions that can be done referencing an item in particular.
    [<RequireQualifiedAccess>]
    type InteractiveItemInteraction =
        | Sleep
        | Play
        | Watch

    /// Interactions related to items in the world or the character's inventory.
    [<RequireQualifiedAccess>]
    type ItemInteraction =
        | Consumable of ConsumableItemInteraction
        | Interactive of InteractiveItemInteraction

    /// Interactions related to moving around the world.
    [<RequireQualifiedAccess>]
    type FreeRoamInteraction =
        /// Allows the player to see what they are currently carrying.
        | Inventory of inventory: Item list
        /// Allows the user to look around and see which objects are available.
        | Look of items: Item list
        /// Allows the player to travel around the current city.
        | Map
        /// Allows the character to use the phone.
        | Phone
        /// Allows waiting.
        | Wait

    /// Interactions that can be done when the character is on the rehearsal room.
    [<RequireQualifiedAccess>]
    type RehearsalInteraction =
        /// Allows to compose new songs.
        | ComposeNewSong
        /// Allows to discard an unfinished song.
        | DiscardSong of songs: UnfinishedSongWithQualities list
        /// Allows to finish an unfinished song.
        | FinishSong of songs: UnfinishedSongWithQualities list
        /// Allows to fire a member of the band.
        | FireMember of members: CurrentMember list
        /// Allows to hire a new member for the band.
        | HireMember
        /// Allows to improve an unfinished song.
        | ImproveSong of songs: UnfinishedSongWithQualities list
        /// Allows to list the members of the band.
        | ListMembers of
            members: CurrentMember list *
            pastMembers: PastMember list
        /// Allows to list all unfinished and finished songs that the band has composed.
        | ListSongs of
            unfinished: UnfinishedSongWithQualities list *
            finished: FinishedSongWithQuality list
        /// Allows to practice a finished song.
        | PracticeSong of songs: FinishedSongWithQuality list

    /// Interactions that can be performed in a bar.
    [<RequireQualifiedAccess>]
    type ShopInteraction =
        /// Allows the character to order a certain item from a selection of
        /// available items in a bar.
        | Order of PurchasableItem list
        /// Allows the character to peek at the available items on the bar.
        | SeeMenu of PurchasableItem list

    /// Interactions that can only be performed inside of a studio.
    [<RequireQualifiedAccess>]
    type StudioInteraction =
        /// Allows to create and record a new album.
        | CreateAlbum of studio: Studio * songs: FinishedSongWithQuality list
        /// Allows to record another song for a previously created album.
        | AddSongToAlbum of
            studio: Studio *
            unreleasedAlbums: UnreleasedAlbum list *
            songs: FinishedSongWithQuality list
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
        | FreeRoam of FreeRoamInteraction
        | Item of ItemInteraction
        | Rehearsal of RehearsalInteraction
        | Shop of ShopInteraction
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

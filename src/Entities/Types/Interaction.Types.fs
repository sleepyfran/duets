namespace Entities

open Entities

[<AutoOpen>]
module InteractionTypes =
    /// Interactions that can be done while on a concert.
    [<RequireQualifiedAccess>]
    type ConcertInteraction =
        | DoEncore of OngoingConcert
        | GetOffStage of OngoingConcert
        | GiveSpeech of OngoingConcert
        | GreetAudience of OngoingConcert
        | FinishConcert of OngoingConcert
        | PlaySong of OngoingConcert

    /// Interactions related to moving around the world.
    [<RequireQualifiedAccess>]
    type FreeRoamInteraction =
        /// Allows going out of the current place towards the given NodeId.
        | GoOut of exit: NodeId * coordinates: ResolvedOutsideCoordinates
        /// Allows movement into the specified direction.
        | Move of direction: Direction * coordinates: NodeCoordinates
        /// Allows the character to use the phone.
        | Phone
        /// Allows waiting.
        | Wait

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
        /// Allows to practice a finished song.
        | PracticeSong of songs: FinishedSongWithQuality list

    /// Interactions that can only be performed inside of a studio.
    [<RequireQualifiedAccess>]
    type StudioInteraction =
        /// Allows to create and record a new album.
        | CreateAlbum of studio: Studio * songs: FinishedSongWithQuality list
        /// Allows to edit the name of a previously recorded album.
        | EditAlbumName of albums: UnreleasedAlbum list
        /// Allows to release a previously recorded album.
        | ReleaseAlbum of albums: UnreleasedAlbum list

    /// Defines all interactions that can be performed in the game. These
    /// interactions are passed back into the CLI layer to actually execute the
    /// flow. Interactions should have all the necessary information for the
    /// CLI to execute a certain action. For example: Move should include the
    /// direction towards which the movement is possible.
    [<RequireQualifiedAccess>]
    type Interaction =
        | Concert of ConcertInteraction
        | FreeRoam of FreeRoamInteraction
        | Rehearsal of RehearsalInteraction
        | Studio of StudioInteraction

    /// Defines all possible reasons why an interaction can be disabled.
    [<RequireQualifiedAccess>]
    type InteractionDisabledReason =
        | NotEnoughEnergy of needed: Energy
        | NotEnoughHealth of needed: Health
        | NotEnoughMood of needed: Mood

    /// Defines the state of an interaction, which can be enabled or disabled
    /// for a specific reason.
    [<RequireQualifiedAccess>]
    type InteractionState =
        | Enabled
        | Disabled of InteractionDisabledReason

    /// Defines an interaction and its state (enabled or disabled).
    type InteractionWithState =
        { Interaction: Interaction
          State: InteractionState }

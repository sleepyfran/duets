namespace Entities

open Entities

[<AutoOpen>]
module InteractionTypes =
    /// Interactions related to moving around the world.
    [<RequireQualifiedAccess>]
    type FreeRoamInteraction =
        /// Allows movement into the specified direction.
        | Move of direction: Direction * id: NodeId
        /// Allows going out of the current place towards the given NodeId.
        | GoOut of exit: NodeId
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
        | HireMember of members: MemberForHire seq
        /// Allows to improve an unfinished song.
        | ImproveSong of songs: UnfinishedSongWithQualities list
        /// Allows to list the members of the band.
        | ListMembers of members: CurrentMember list
        /// Allows to practice a finished song.
        | PracticeSong of songs: FinishedSongWithQuality list

    /// Interactions that can only be performed inside of a studio.
    [<RequireQualifiedAccess>]
    type StudioInteraction =
        /// Allows to create and record a new album.
        | CreateAlbum
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
        | FreeRoam of FreeRoamInteraction
        | Rehearsal of RehearsalInteraction
        | Studio of StudioInteraction

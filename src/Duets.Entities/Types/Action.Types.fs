namespace Duets.Entities

[<AutoOpen>]
module ActionTypes =
    /// Defines all the actions that are available in the game, with their
    /// associated payload types. All actions should be prefixed by the context
    /// in which they happen, for example: boarding a plane can only happen
    /// in the airport, so the action should be called AirportBoardPlane.
    type Action =
        | AirportBoardPlane of Flight
        | AirportPassSecurity
        | AirportWaitForLanding of Flight
        | RehearsalRoomPracticeSong of {| Band: Band; Song: Finished<Song> |}
        | StudioStartAlbum of
            {| Studio: Studio
               SelectedProducer: SelectedProducer
               Band: Band
               AlbumName: string
               FirstSong: Finished<Song> |}
        | StudioRecordSongForAlbum of
            {| Studio: Studio
               Band: Band
               Album: UnreleasedAlbum
               Song: Finished<Song> |}
        | StudioReleaseAlbum of {| Band: Band; Album: UnreleasedAlbum |}
        | StudioRenameAlbum of
            {| Album: UnreleasedAlbum
               Band: Band
               Name: string |}

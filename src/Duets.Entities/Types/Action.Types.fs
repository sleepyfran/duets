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
        | ConcertStart of
            {| Band: Band
               Concert: ScheduledConcert |}
        | ConcertPerformAction of
            {| Action: ConcertEvent
               Concert: OngoingConcert |}
        | ConcertEncore of OngoingConcert
        | GymPayEntranceFee of EntranceFee: Amount
        | RehearsalRoomComposeSong of {| Band: Band; Song: Song |}
        | RehearsalRoomDiscardSong of {| Band: Band; Song: Unfinished<Song> |}
        | RehearsalRoomFireMember of
            {| Band: Band
               CurrentMember: CurrentMember |}
        | RehearsalRoomFinishSong of {| Band: Band; Song: Unfinished<Song> |}
        | RehearsalRoomHireMember of
            {| Band: Band
               MemberToHire: MemberForHire |}
        | RehearsalRoomImproveSong of {| Band: Band; Song: Unfinished<Song> |}
        | RehearsalRoomPracticeSong of {| Band: Band; Song: Finished<Song> |}
        | RehearsalRoomSwitchToGenre of {| Band: Band; Genre: Genre |}
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
        | WorkShift of Job

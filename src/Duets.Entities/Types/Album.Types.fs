namespace Duets.Entities

[<AutoOpen>]
module AlbumTypes =
    /// Unique identifier of an album.
    type AlbumId = AlbumId of Identity

    /// Represents the different types of albums that can be done. Depends
    /// on the amount of songs and the length of those in the album.
    type AlbumType =
        | Single
        | EP
        | LP

    /// Defines the track list of an album as a reference to the song in the
    /// repertoire.
    type TrackListRef = Recorded<SongId> list

    /// Defines the track list of an album after all the songs have been
    /// retrieved from the repertoire.
    type TrackList = Recorded<Song> list

    /// Represents a band's album.
    type Album =
        { Id: AlbumId
          BandId: BandId
          Name: string
          Genre: Genre
          TrackList: TrackListRef
          Type: AlbumType }

    /// Represents which producer was chosen to take care of the album production,
    /// mixing and mastering.
    [<RequireQualifiedAccess>]
    type SelectedProducer =
        | StudioProducer
        | PlayableCharacter

    /// Defines an album that was recorded but hasn't been released.
    type UnreleasedAlbum =
        { Album: Album
          SelectedProducer: SelectedProducer }

    /// Identifies a review source for albums.
    type ReviewerId =
        | Metacritic
        | Pitchfork
        | RateYourMusic
        | SputnikMusic

    /// Defines a review of an album
    type Review =
        {
            Reviewer: ReviewerId
            /// Score from 0 to 100.
            Score: int
        }

    /// Defines an album that has been released, with the maximum amounts of
    /// daily streams that the album can have plus the current hype of the album
    /// which together determine the amount of streams that it will have.
    type ReleasedAlbum =
        {
            Album: Album
            ReleaseDate: Date
            /// Total amount of streams that the album has had so far.
            Streams: int
            /// Represents a sudden burst of interest in the album. When freshly
            /// released, it'll be 1.0 unless some advertisement was done to
            /// increase it and can fluctuate depending on actions that the band
            /// does. This is used when calculating the daily streams.
            Hype: float
            Reviews: Review list
        }

    /// Collection of albums (either released or unreleased) by a band.
    type AlbumsByBand<'album> = Map<BandId, Map<AlbumId, 'album>>

    /// Represents both the unreleased and released albums that the band has.
    type BandAlbumRepertoire =
        { UnreleasedAlbums: AlbumsByBand<UnreleasedAlbum>
          ReleasedAlbums: AlbumsByBand<ReleasedAlbum> }

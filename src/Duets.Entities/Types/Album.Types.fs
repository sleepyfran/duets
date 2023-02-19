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

    /// Represents a band's album.
    type Album =
        { Id: AlbumId
          Name: string
          TrackList: RecordedSong list
          Type: AlbumType }

    /// Defines an album that was recorded but hasn't been released.
    type UnreleasedAlbum = UnreleasedAlbum of Album

    /// Defines an album that has been released, with the maximum amounts of
    /// daily streams that the album can have plus the current hype of the album
    /// which together determine the amount of streams that it will have.
    type ReleasedAlbum =
        { Album: Album
          ReleaseDate: Date
          /// Total amount of streams that the album has had so far.
          Streams: int
          /// Represents a sudden burst of interest in the album. When freshly
          /// released, it'll be 1.0 unless some advertisement was done to
          /// increase it and can fluctuate depending on actions that the band
          /// does. This is used when calculating the daily streams.
          Hype: float }

    /// Collection of albums (either released or unreleased) by a band.
    type AlbumsByBand<'album> = Map<BandId, Map<AlbumId, 'album>>

    /// Represents both the unreleased and released albums that the band has.
    type BandAlbumRepertoire =
        { UnreleasedAlbums: AlbumsByBand<UnreleasedAlbum>
          ReleasedAlbums: AlbumsByBand<ReleasedAlbum> }
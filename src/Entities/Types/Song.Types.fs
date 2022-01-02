namespace Entities

open FSharp.Data.UnitSystems.SI.UnitNames

[<AutoOpen>]
module SongTypes =
    /// Unique identifier of a song.
    type SongId = SongId of Identity

    /// Defines the different styles of vocals that a song can have.
    type VocalStyle =
        | Instrumental
        | Normal
        | Growl
        | Screamo

    /// Length of a song or an album as minutes and seconds.
    type Length =
        { Minutes: int<minute>
          Seconds: int<second> }

    [<Measure>]
    type practice

    type Practice = int<practice>

    /// Defines a song composed by a band in its most basic form, there's more
    /// specific types depending on the type of information we want to query.
    type Song =
        { Id: SongId
          Name: string
          Length: Length
          VocalStyle: VocalStyle
          Genre: Genre
          Practice: Practice }

    /// Defines a song that is still being developed by the band.
    type UnfinishedSong = UnfinishedSong of Song
    /// Defines a song that has been consider finished and it's part of the band's
    /// repertoire and cannot be further developed anymore.
    type FinishedSong = FinishedSong of Song

    /// Shapes a relation between an unfinished song, its max quality and the
    /// current quality.
    type UnfinishedSongWithQualities = UnfinishedSong * MaxQuality * Quality
    /// Shapes a relation between a finished song and its quality.
    type FinishedSongWithQuality = FinishedSong * Quality
    /// Shapes a relation between a finished song and the combination of the
    /// quality of the song itself with the quality of the production.
    type RecordedSong = FinishedSong * Quality

    /// Indicates whether the song can be further improved or if it has reached its
    /// maximum quality and thus cannot be improved. All variants wrap an int that
    /// hold the current value.
    type SongStatus =
        | CanBeImproved
        | ReachedMaxQualityInLastImprovement
        | ReachedMaxQualityAlready

    /// Collection of songs (either finished or unfinished) by a band.
    type SongsByBand<'song> = Map<BandId, Map<SongId, 'song>>

    /// Represents the repertoire of a band with its unfinished and finished songs.
    /// Only finished songs can be recorded and played live.
    type BandSongRepertoire =
        { UnfinishedSongs: SongsByBand<UnfinishedSongWithQualities>
          FinishedSongs: SongsByBand<FinishedSongWithQuality> }

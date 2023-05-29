namespace Duets.Entities

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

    /// Defines a song that has been started but not finished yet. It accepts
    /// a generic parameter so that we can hold either the entire song object
    /// or just the ID.
    type Unfinished<'s> =
        | Unfinished of song: 's * max: MaxQuality * current: Quality

    /// Defines a song that has been finished and can be recorded and played live.
    type Finished<'s> = Finished of song: 's * quality: Quality

    /// Defines a song that has been finished with whether it has been recorded
    /// or not.
    type FinishedWithRecordingStatus<'s> =
        | FinishedWithRecordingStatus of song: Finished<'s> * recorded: bool

    /// Defines a song that has been recorded and can be played live.
    type Recorded<'s> = Recorded of song: 's * totalQuality: Quality

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
        { UnfinishedSongs: SongsByBand<Unfinished<Song>>
          FinishedSongs: SongsByBand<FinishedWithRecordingStatus<Song>> }

module Entities.Song

open System
open Entities.Identity

type SongId = SongId of Identity

/// Defines the different styles of vocals that a song can have.
type VocalStyle =
  | Instrumental
  | Normal
  | Growl
  | Screamo

/// Defines a song composed by a band in its most basic form, there's more
/// specific types depending on the type of information we want to query.
type Song =
  { Id: SongId
    Name: string
    Length: int
    VocalStyle: VocalStyle }

/// Defines a song that is still being developed by the band.
type UnfinishedSong = UnfinishedSong of Song
/// Defines a song that has been consider finished and it's part of the band's
/// repertoire and cannot be further developed anymore.
type FinishedSong = FinishedSong of Song

/// Defines the current quality of the song
type Quality = Quality of int
/// Defines the max quality achievable by the band for this song.
type MaxQuality = MaxQuality of int
/// Shapes a relation between an unfinished song, its max quality and the
/// current quality.
type UnfinishedWithQualities = UnfinishedSong * MaxQuality * Quality
/// Shapes a relation between a finished song and its quality.
type FinishedWithQuality = FinishedSong * Quality

let empty =
  { Id = SongId(Guid.NewGuid())
    Name = ""
    Length = 0
    VocalStyle = VocalStyle.Instrumental }

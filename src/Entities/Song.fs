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

module VocalStyle =
  /// Returns a VocalStyle given its string representation. If no match is found,
  /// normal is returned instead.
  let from str =
    match str with
    | "Instrumental" -> Instrumental
    | "Growl" -> Growl
    | "Screamo" -> Screamo
    | _ -> Normal

/// Defines a song composed by a band in its most basic form, there's more
/// specific types depending on the type of information we want to query.
type Song =
  { Id: SongId
    Name: string
    Length: int
    VocalStyle: VocalStyle }

/// Indicates whether the song can be further improved or if it has reached its
/// maximum quality and thus cannot be improved. All variants wrap an int that
/// hold the current value.
type SongStatus =
  | CanBeImproved of int
  | ReachedMaxQuality of int

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
type UnfinishedSongWithQualities = UnfinishedSong * MaxQuality * Quality
/// Shapes a relation between a finished song and its quality.
type FinishedWithQuality = FinishedSong * Quality

type SongValidationError =
  | NameTooShort
  | NameTooLong
  | LengthTooShort
  | LengthTooLong

let private TwentySeconds = 20
let private ThirtyMinutes = 60 * 30

/// Creates a song given a name, length and vocal style, if possible.
let from (name: string) length vocalStyle =
  if name.Length < 1 then
    Error NameTooShort
  else if name.Length > 50 then
    Error NameTooLong
  else if length < TwentySeconds then
    Error LengthTooShort
  else if length > ThirtyMinutes then
    Error LengthTooLong
  else
    Ok
      { Id = SongId <| create ()
        Name = name
        Length = length
        VocalStyle = vocalStyle }

/// Creates an empty song with all its fields set to a default value.
let empty =
  { Id = SongId <| create ()
    Name = ""
    Length = 0
    VocalStyle = VocalStyle.Instrumental }

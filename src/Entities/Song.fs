module Entities.Song

open FSharp.Data.UnitSystems.SI.UnitNames

type SongValidationError =
    | NameTooShort
    | NameTooLong
    | LengthTooShort
    | LengthTooLong

let private TwentySeconds = 20<second>
let private ThirtyMinutes = 60<second> * 30

/// Creates a song given a name, length and vocal style, if possible.
let from (name: string) (length: int<second>) vocalStyle genre =
    if name.Length < 1 then
        Error NameTooShort
    else if name.Length > 100 then
        Error NameTooLong
    else if length < TwentySeconds then
        Error LengthTooShort
    else if length > ThirtyMinutes then
        Error LengthTooLong
    else
        Ok
            { Id = SongId <| Identity.create ()
              Name = name
              Length = length
              VocalStyle = vocalStyle
              Genre = genre }

/// Returns the song contained in an unfinished song.
let fromUnfinished (UnfinishedSong (song), _, _) = song

/// Returns the song contained in a finished song.
let fromFinished (FinishedSong (song), _) = song

/// Creates an empty song with all its fields set to a default value.
let empty =
    { Id = SongId <| Identity.create ()
      Name = ""
      Length = 0<second>
      VocalStyle = VocalStyle.Instrumental
      Genre = "" }

module VocalStyle =
    /// Returns a VocalStyle given its string representation. If no match is found,
    /// normal is returned instead.
    let from str =
        match str with
        | "Instrumental" -> Instrumental
        | "Growl" -> Growl
        | "Screamo" -> Screamo
        | _ -> Normal

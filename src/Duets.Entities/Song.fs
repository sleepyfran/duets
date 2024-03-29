module Duets.Entities.Song

open FSharp.Data.UnitSystems.SI.UnitNames
open Duets.Entities.Time

type SongNameValidationError =
    | NameTooShort
    | NameTooLong

type SongLengthValidationError =
    | LengthTooShort
    | LengthTooLong

let private TwentySeconds = 20<second>
let private ThirtyMinutes = 60<second> * 30

/// Validates that the name between 1 and 100 characters.
let validateName name =
    match String.length name with
    | l when l < 1 -> Error NameTooShort
    | l when l > 100 -> Error NameTooLong
    | _ -> Ok name

/// Validates that the length is more than 20 seconds and less than 30 minutes.
let validateLength length =
    match Length.inSeconds length with
    | l when l < TwentySeconds -> Error LengthTooShort
    | l when l > ThirtyMinutes -> Error LengthTooLong
    | _ -> Ok length

/// Creates a song given a name, length and vocal style, if possible.
let from (name: string) length vocalStyle =
    { Id = SongId <| Identity.create ()
      Name = name
      Length = length
      VocalStyle = vocalStyle
      Practice = 20<practice> }

/// Returns the song contained in an unfinished song.
let fromUnfinished (Unfinished(song, _, _)) = song

/// Returns the song contained in a finished song.
let fromFinished (Finished(song, _)) = song

/// Creates an empty song with all its fields set to a default value.
let empty =
    { Id = SongId <| Identity.create ()
      Name = ""
      Length = Length.empty
      VocalStyle = VocalStyle.Instrumental
      Practice = 0<practice> }

module Finished =
    /// Returns the song contained in a finished song that has a status attached.
    let fromFinishedWithStatus (FinishedWithMetadata(song, _, _)) = song

    /// Returns a finished song with metadata about its recording status and
    /// the date when the song was finished.
    let attachMetadata recorded finishDate song =
        FinishedWithMetadata(song, recorded, finishDate)

    module Metadata =
        /// Returns the recording status of a finished song.
        let recorded (FinishedWithMetadata(_, recorded, _)) = recorded

        /// Returns the date when a finished song was finished.
        let finishDate (FinishedWithMetadata(_, _, finishDate)) = finishDate

module VocalStyle =
    /// Returns a VocalStyle given its string representation. If no match is found,
    /// normal is returned instead.
    let from str =
        match str with
        | "Instrumental" -> Instrumental
        | "Growl" -> Growl
        | "Screamo" -> Screamo
        | _ -> VocalStyle.Normal

module Entities.Album

open FSharp.Data.UnitSystems.SI.UnitNames

type CreationError =
    | NameTooShort
    | NameTooLong
    | NoSongsSelected

let private twentyFiveMinutes = 25 * 60<second>

/// Determines the length of the given track list.
let length trackList =
    List.fold
        (fun albumLength ((FinishedSong s), _) -> albumLength + s.Length)
        0<second>
        trackList

/// Determines the record type of an album given its track list.
let recordType trackList =
    if List.length trackList = 1 then
        Single
    else
        match length trackList with
        | l when l <= twentyFiveMinutes -> EP
        | _ -> LP

/// Creates an album given its name and the list of songs that define the track
/// list.
let from (name: string) (trackList: RecordedSong list) =
    if name.Length < 1 then
        Error NameTooShort
    else if name.Length > 100 then
        Error NameTooLong
    else if List.isEmpty trackList then
        Error NoSongsSelected
    else
        Ok
            { Id = AlbumId <| Identity.create ()
              Name = name
              TrackList = trackList
              Type = recordType trackList }

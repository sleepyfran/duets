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

let private validateName name =
    match String.length name with
    | l when l < 1 -> Error NameTooShort
    | l when l > 100 -> Error NameTooLong
    | _ -> Ok()

let private validateTrackList trackList =
    match List.isEmpty trackList with
    | true -> Error NoSongsSelected
    | _ -> Ok()

/// Creates an album given its name and the list of songs that define the track
/// list.
let from (name: string) (trackList: RecordedSong list) =
    validateName name
    |> Result.bind (fun _ -> validateTrackList trackList)
    |> Result.bind
        (fun _ ->
            Ok
                { Id = AlbumId <| Identity.create ()
                  Name = name
                  TrackList = trackList
                  Type = recordType trackList })

/// Modifies the name of the given album validating that it's correct.
let modifyName (UnreleasedAlbum album) name =
    validateName name
    |> Result.bind (fun _ -> Ok <| UnreleasedAlbum { album with Name = name })

module Released =
    /// Transforms a given unreleased album into its released status.
    let fromUnreleased (UnreleasedAlbum album) releaseDate =
        ReleasedAlbum(album, releaseDate)

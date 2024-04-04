module Duets.Entities.Album

open Duets.Common
open FSharp.Data.UnitSystems.SI.UnitNames

let private twentyFiveMinutes = 25 * 60<second>

/// Determines the length of the given track list.
let lengthInSeconds (trackList: TrackList) =
    List.fold
        (fun albumLength (Recorded(song, _)) ->
            albumLength + Time.Length.inSeconds song.Length)
        0<second>
        trackList

type NameError =
    | NameTooShort
    | NameTooLong

type RecordTypeError = EmptyTrackList

/// Determines the record type of an album given its track list.
let recordType trackList =
    if List.length trackList = 0 then
        Error EmptyTrackList
    else if List.length trackList = 1 then
        Ok Single
    else
        match lengthInSeconds trackList with
        | l when l <= twentyFiveMinutes -> Ok EP
        | _ -> Ok LP

/// Validates that the record name is not below 1 character or more
/// than 100.
let validateName name =
    match String.length name with
    | l when l < 1 -> Error NameTooShort
    | l when l > 100 -> Error NameTooLong
    | _ -> Ok name

/// Creates an album given its name and the initial song of the track-list.
let from (band: Band) name initialSong =
    { Id = AlbumId <| Identity.create ()
      BandId = band.Id
      Name = name
      Genre = band.Genre
      TrackList = [ initialSong ]
      Type = Single }

/// Adds the given song to the album and recomputes the album type.
let updateTrackList album (trackList: TrackList) =
    let updatedTrackList =
        trackList
        |> List.map (fun (Recorded(song, quality)) ->
            Recorded(song.Id, quality))

    { album with
        TrackList = updatedTrackList
        Type = recordType trackList |> Result.unwrap }

/// Returns the inner album of an unreleased album.
let fromUnreleased (unreleasedAlbum: UnreleasedAlbum) = unreleasedAlbum.Album

/// Returns the inner album of a released album.
let fromReleased releasedAlbum = releasedAlbum.Album

module Unreleased =
    /// Creates an unreleased album given a name and a track list.
    let from band name trackList producer =
        { Album = from band name trackList
          SelectedProducer = producer }

    /// Modifies the name of the given album validating that it's correct.
    let modifyName (unreleasedAlbum: UnreleasedAlbum) name =
        { unreleasedAlbum with
            UnreleasedAlbum.Album.Name = name }

module Released =
    /// Updates an already released album with the new amount of streams and
    /// hype.
    let update album streams hype =
        { album with
            Streams = streams
            Hype = hype }

    /// Transforms a given unreleased album into its released status.
    let fromUnreleased (unreleasedAlbum: UnreleasedAlbum) releaseDate hype =
        { Album = unreleasedAlbum.Album
          ReleaseDate = releaseDate
          Streams = 0
          Hype = hype
          Reviews = [] }

module Review =
    /// Creates a review given a reviewer and a score, making sure the score
    /// stays between 0 and 100.
    let create reviewer score =
        { Reviewer = reviewer
          Score = score |> Math.clamp 0 100 }

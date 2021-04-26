module Core.Songs.Queries

open Entities.Song
open Storage.State

/// Returns all unfinished songs by all bands from the character.
let unfinishedSongs () =
  getState () |> fun state -> state.UnfinishedSongs

/// Returns all unfinished songs by the given band. If no unfinished songs
/// could be found, returns an empty list.
let unfinishedSongsByBand bandId =
  unfinishedSongs ()
  |> Map.tryFind bandId
  |> Option.defaultValue []

/// Returns a specific song given the ID of the band that holds it and the ID
/// of the song to retrieve.
let unfinishedSongByBandAndSongId bandId songId =
  unfinishedSongsByBand bandId
  |> List.tryFind (fun ((UnfinishedSong (song)), _, _) -> song.Id = songId)

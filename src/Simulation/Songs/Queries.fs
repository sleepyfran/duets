module Simulation.Songs.Queries

open Lenses
open Storage.State

/// Returns all unfinished songs by all bands from the character.
let unfinishedSongs () =
  getState () |> Lens.get unfinishedSongsLenses

/// Returns all finished songs by all bands from the character.
let finishedSongs () =
  getState () |> Lens.get finishedSongsLenses

let tryFindBandOrDefault bandId map =
  map
  |> Map.tryFind bandId
  |> Option.defaultValue Map.empty

/// Returns all unfinished songs by the given band. If no unfinished songs
/// could be found, returns an empty map.
let unfinishedSongsByBand bandId =
  unfinishedSongs () |> tryFindBandOrDefault bandId

/// Returns all finished songs by the given band. If no finished songs could
/// be found, returns an empty map.
let finishedSongsByBand bandId =
  finishedSongs () |> tryFindBandOrDefault bandId

/// Returns a specific song given the ID of the band that holds it and the ID
/// of the song to retrieve.
let unfinishedSongByBandAndSongId bandId songId =
  unfinishedSongsByBand bandId |> Map.tryFind songId

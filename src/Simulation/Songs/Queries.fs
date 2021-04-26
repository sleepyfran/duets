module Simulation.Songs.Queries

open Entities
open Storage.State

/// Returns all unfinished songs by all bands from the character.
let unfinishedSongs () =
  getState () |> fun state -> state.BandRepertoire.Unfinished

/// Returns all unfinished songs by the given band. If no unfinished songs
/// could be found, returns an empty map.
let unfinishedSongsByBand bandId =
  unfinishedSongs ()
  |> Map.tryFind bandId
  |> Option.defaultValue Map.empty

/// Returns a specific song given the ID of the band that holds it and the ID
/// of the song to retrieve.
let unfinishedSongByBandAndSongId bandId songId =
  unfinishedSongsByBand bandId |> Map.tryFind songId

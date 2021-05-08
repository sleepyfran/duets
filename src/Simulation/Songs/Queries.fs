module Simulation.Songs.Queries

open Aether
open Simulation.Songs
open Storage

/// Returns all unfinished songs by the given band. If no unfinished songs
/// could be found, returns an empty map.
let unfinishedSongsByBand state bandId =
  let unfinishedSongLens = Lenses.unfinishedSongs_ bandId

  state
  |> Optic.get unfinishedSongLens
  |> Option.defaultValue Map.empty

/// Returns all finished songs by the given band. If no finished songs could
/// be found, returns an empty map.
let finishedSongsByBand state bandId =
  let finishedSongLens = Lenses.finishedSongs_ bandId

  state
  |> Optic.get finishedSongLens
  |> Option.defaultValue Map.empty

/// Returns a specific song given the ID of the band that holds it and the ID
/// of the song to retrieve.
let unfinishedSongByBandAndSongId state bandId songId =
  unfinishedSongsByBand state bandId |> Map.tryFind songId

module Simulation.Songs.Queries

open Aether
open Entities

/// Returns all unfinished songs by the given band. If no unfinished songs
/// could be found, returns an empty map.
let unfinishedSongsByBand state bandId =
  let unfinishedSongLens =
    Lenses.FromState.Songs.unfinishedByBand_ bandId

  state
  |> Optic.get unfinishedSongLens
  |> Option.defaultValue Map.empty

/// Returns all finished songs by the given band. If no finished songs could
/// be found, returns an empty map.
let finishedSongsByBand state bandId =
  let finishedSongLens =
    Lenses.FromState.Songs.finishedByBand_ bandId

  state
  |> Optic.get finishedSongLens
  |> Option.defaultValue Map.empty

/// Returns a specific song given the ID of the band that holds it and the ID
/// of the song to retrieve.
let unfinishedSongByBandAndSongId state bandId songId =
  unfinishedSongsByBand state bandId
  |> Map.tryFind songId

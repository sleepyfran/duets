module Simulation.Songs.Composition.FinishSong

open Common
open Entities

/// Orchestrates the finishing of a song, which moves it from the map of
/// unfinished songs into the map of finished songs.
let finishSong band (unfinishedSong: UnfinishedSongWithQualities) =
  let ((UnfinishedSong (song)), _, quality) = unfinishedSong

  (FinishedSong song, quality)
  |> Tuple.two band
  |> SongFinished

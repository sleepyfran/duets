module Simulation.Songs.Composition.FinishSong

open Simulation.Bands.Queries
open Simulation.Songs.Composition.Common

/// Orchestrates the finishing of a song, which moves it from the map of
/// unfinished songs into the map of finished songs.
let finishSong unfinishedSong =
  let band = currentBand ()

  unfinishedSong
  |> removeUnfinishedSong band
  |> addFinishedSong band

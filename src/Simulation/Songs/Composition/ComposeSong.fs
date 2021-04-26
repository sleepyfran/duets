module Simulation.Songs.Composition.ComposeSong

open Simulation.Songs.Composition.Common
open Simulation.Bands.Queries
open Entities

/// Orchestrates the song composition, which calculates the qualities of a song
/// and adds them with the song to the band's unfinished songs.
let composeSong song =
  let band = currentBand ()
  let maximumQuality = qualityForBand band

  let initialQuality =
    calculateQualityIncreaseOf maximumQuality

  let songWithQualities =
    (UnfinishedSong song, maximumQuality, initialQuality)

  addUnfinishedSong songWithQualities band

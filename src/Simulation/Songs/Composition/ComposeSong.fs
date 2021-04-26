module Simulation.Songs.Composition.ComposeSong

open Simulation.Songs.Composition.Common
open Simulation.Bands.Queries
open Entities.Song

/// Orchestrates the song composition, which calculates the qualities of a song
/// and adds them with the song to the band's unfinished songs.
let composeSong song =
  let band = currentBand ()
  let maximumQuality = qualityForBand band

  let initialQuality =
    calculateQualityIncreaseOf maximumQuality

  let songWithQualities =
    (UnfinishedSong song, MaxQuality maximumQuality, Quality initialQuality)

  addUnfinishedSong songWithQualities band

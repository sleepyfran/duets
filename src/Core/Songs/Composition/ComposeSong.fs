module Core.Songs.Composition.ComposeSong

open Core.Songs.Composition.Common
open Core.Bands.Queries
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

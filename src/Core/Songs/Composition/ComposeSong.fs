module Core.Songs.Composition.ComposeSong

open Core.Songs.Composition.Common
open Entities.Song
open Mediator.Queries.Storage
open Mediator.Query

/// Orchestrates the song composition, which calculates the qualities of a song
/// and adds them with the song to the band's unfinished songs.
let composeSong input =
  let band = query CurrentBandQuery
  let maximumQuality = qualityForBand band

  let initialQuality =
    calculateQualityIncreaseOf maximumQuality

  let songWithQualities =
    (UnfinishedSong input, MaxQuality maximumQuality, Quality initialQuality)

  addUnfinishedSong songWithQualities band

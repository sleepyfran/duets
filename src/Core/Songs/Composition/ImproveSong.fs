module Core.Songs.Composition.ImproveSong

open Core.Bands.Queries
open Core.Songs.Composition.Common
open Entities.Song

let private doImprove song maxQuality quality =
  let band = currentBand ()
  let increase = calculateQualityIncreaseOf maxQuality
  let updatedQuality = quality + increase
  let canBeFurtherImproved = maxQuality >= updatedQuality

  let songWithUpdatedQualities =
    (song, MaxQuality maxQuality, Quality updatedQuality)

  addUnfinishedSong songWithUpdatedQualities band

  if canBeFurtherImproved then
    CanBeImproved updatedQuality
  else
    ReachedMaxQuality updatedQuality

/// Orchestrates the improvement of a song, which calculates the increase that
/// should happen in this action and returns whether the song can be further
/// increased or not.
let improveSong (song, MaxQuality maxQuality, Quality currentQuality) =
  if currentQuality >= maxQuality then
    ReachedMaxQuality currentQuality
  else
    doImprove song maxQuality currentQuality

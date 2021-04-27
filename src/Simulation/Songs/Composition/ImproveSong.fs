module Simulation.Songs.Composition.ImproveSong

open Simulation.Bands.Queries
open Simulation.Songs.Composition.Common
open Entities

let private doImprove song maxQuality quality =
  let band = currentBand ()
  let increase = calculateQualityIncreaseOf maxQuality
  let updatedQuality = quality + increase
  let canBeFurtherImproved = maxQuality >= updatedQuality

  let songWithUpdatedQualities = (song, maxQuality, updatedQuality)

  addUnfinishedSong band songWithUpdatedQualities

  if canBeFurtherImproved then
    CanBeImproved updatedQuality
  else
    ReachedMaxQuality updatedQuality

/// Orchestrates the improvement of a song, which calculates the increase that
/// should happen in this action and returns whether the song can be further
/// increased or not.
let improveSong (song, maxQuality, currentQuality) =
  if currentQuality >= maxQuality then
    ReachedMaxQuality currentQuality
  else
    doImprove song maxQuality currentQuality

module Simulation.Songs.Composition.ImproveSong

open Common
open Entities
open Simulation.Time

let private improveSong' state band song maxQuality (quality: Quality) =
    let songBeforeUpgrade = (song, maxQuality, quality)
    let increase = calculateQualityIncreaseOf maxQuality

    let updatedQuality =
        quality + increase |> Math.clamp 0<quality> 100<quality>

    let canBeFurtherImproved =
        maxQuality > updatedQuality && updatedQuality < 100<quality>

    let songWithUpdatedQualities = (song, maxQuality, updatedQuality)

    let effects =
        [ SongImproved(band, Diff(songBeforeUpgrade, songWithUpdatedQualities))
          yield! AdvanceTime.advanceDayMoment' state 1<dayMoments> ]

    if canBeFurtherImproved then
        (CanBeImproved, effects)
    else
        (ReachedMaxQualityInLastImprovement, effects)

/// Orchestrates the improvement of a song, which calculates the increase that
/// should happen in this action and returns whether the song can be further
/// increased or not.
let improveSong state band (song, maxQuality, currentQuality) =
    if currentQuality >= maxQuality then
        (ReachedMaxQualityAlready, [])
    else
        improveSong' state band song maxQuality currentQuality

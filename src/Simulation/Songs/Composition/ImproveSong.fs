module Simulation.Songs.Composition.ImproveSong

open Common
open Entities

let private doImprove band song maxQuality quality =
    let songBeforeUpgrade = (song, maxQuality, quality)
    let increase = calculateQualityIncreaseOf maxQuality
    let updatedQuality = quality + increase
    let canBeFurtherImproved = maxQuality > updatedQuality

    let songWithUpdatedQualities = (song, maxQuality, updatedQuality)

    let status =
        if canBeFurtherImproved then
            CanBeImproved updatedQuality
        else
            ReachedMaxQualityInLastImprovement updatedQuality

    SongImproved(band, Diff(songBeforeUpgrade, songWithUpdatedQualities))
    |> Effect.single
    |> Tuple.two status

/// Orchestrates the improvement of a song, which calculates the increase that
/// should happen in this action and returns whether the song can be further
/// increased or not.
let improveSong band (song, maxQuality, currentQuality) =
    if currentQuality >= maxQuality then
        (ReachedMaxQualityAlready currentQuality, Effect.empty)
    else
        doImprove band song maxQuality currentQuality

module Simulation.Songs.Composition.ImproveSong

open Common
open Entities
open Simulation.Skills.ImproveSkills

let private improveSong' band song maxQuality quality =
    let songBeforeUpgrade = (song, maxQuality, quality)
    let increase = calculateQualityIncreaseOf maxQuality
    let updatedQuality = quality + increase
    let canBeFurtherImproved = maxQuality > updatedQuality

    let songWithUpdatedQualities = (song, maxQuality, updatedQuality)

    let effect =
        SongImproved(band, Diff(songBeforeUpgrade, songWithUpdatedQualities))

    if canBeFurtherImproved then
        CanBeImproved effect
    else
        ReachedMaxQualityInLastImprovement effect

/// Orchestrates the improvement of a song, which calculates the increase that
/// should happen in this action and returns whether the song can be further
/// increased or not.
let improveSong band (song, maxQuality, currentQuality) =
    if currentQuality >= maxQuality then
        ReachedMaxQualityAlready
    else
        improveSong' band song maxQuality currentQuality

module Duets.Simulation.Songs.Composition.ImproveSong

open Common
open Duets.Common
open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Time

let private improveSong' state band song maxQuality (quality: Quality) =
    let songBeforeUpgrade = Unfinished(song, maxQuality, quality)
    let increase = calculateQualityIncreaseOf songBeforeUpgrade

    let updatedQuality =
        quality + increase |> Math.clamp 0<quality> 100<quality>

    let canBeFurtherImproved =
        maxQuality > updatedQuality && updatedQuality < 100<quality>

    let songWithUpdatedQualities = Unfinished(song, maxQuality, updatedQuality)

    // TODO: Move these to the common time handler once we move completely to actions.
    let timeEffects =
        RehearsalInteraction.ImproveSong []
        |> Interaction.Rehearsal
        |> Queries.InteractionTime.timeRequired
        |> AdvanceTime.advanceDayMoment' state

    let improveEffect =
        if canBeFurtherImproved then
            SongImproved(
                band,
                Diff(songBeforeUpgrade, songWithUpdatedQualities)
            )
        else
            SongImprovedToMax(
                band,
                Diff(songBeforeUpgrade, songWithUpdatedQualities)
            )

    improveEffect :: timeEffects |> Ok

/// Orchestrates the improvement of a song, which calculates the increase that
/// should happen in this action and returns whether the song can be further
/// increased or not.
let improveSong state band unfinishedSong =
    let (Unfinished(song, maxQuality, currentQuality)) = unfinishedSong

    if currentQuality >= maxQuality then
        SongAlreadyImprovedToMax unfinishedSong |> Error
    else
        improveSong' state band song maxQuality currentQuality

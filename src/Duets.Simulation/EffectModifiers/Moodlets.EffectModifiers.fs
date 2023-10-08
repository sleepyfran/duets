module rec Duets.Simulation.EffectModifiers.Moodlets

open Duets.Common
open Duets.Entities
open Duets.Simulation

/// Applies any needed modifications for effects based on the moodlets
let modify state effect =
    let moodlets = Queries.Characters.playableCharacterMoodlets state

    moodlets
    |> Set.fold
        (fun effect moodlet ->
            match moodlet.MoodletType with
            | MoodletType.NotInspired -> modifyForNotInspired effect)
        effect

/// Modifies the SongStarted and SongImproved effects to reduce the quality
/// of the songs while the character is not inspired.
let private modifyForNotInspired effect =
    let modify q =
        float q * Config.Moodlets.NotInspired.songCompositionReduction
        |> Math.ceilToNearest
        |> (*) 1<quality>

    match effect with
    | SongStarted(band, Unfinished(song, maxQuality, currentQuality)) ->
        let maxQuality = modify maxQuality
        let currentQuality = modify currentQuality

        SongStarted(band, Unfinished(song, maxQuality, currentQuality))
    | SongImproved(band,
                   Diff(before, Unfinished(song, maxQuality, currentQuality))) ->
        let currentQuality = modify currentQuality

        SongImproved(
            band,
            Diff(before, Unfinished(song, maxQuality, currentQuality))
        )
    | _ -> effect

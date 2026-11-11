module Duets.Simulation.Songs.Composition.Common

open Duets.Simulation.Queries
open Duets.Entities
open Duets.Entities.Skill

/// Computes the score associated with each member of the band for the song.
let qualityForMember state genre (currentMember: CurrentMember) =
    let skillLevel skillId =
        Skills.characterSkillWithLevel state currentMember.CharacterId skillId
        |> snd

    let instrumentLevel =
        currentMember.Roles
        |> List.sumBy (SkillId.Instrument >> skillLevel)
        |> fun total -> total / currentMember.Roles.Length

    let influencingLevels =
        [ skillLevel SkillId.Composition
          instrumentLevel
          skillLevel (SkillId.Genre genre) ]

    influencingLevels
    |> List.sum
    |> fun total -> total / influencingLevels.Length

/// Computes the maximum score that the band can achieve for a song in a moment
/// in time given each member's skills in composition, the current genre and
/// the member's instrument skill.
let qualityForBand state band =
    band.Members
    |> List.map (qualityForMember state band.Genre)
    |> List.sum
    |> fun score -> score / band.Members.Length
    |> fun score ->
        match score with
        | score when score < 5 -> 5<quality>
        | _ -> score * 1<quality>

/// Calculates at what rate the score of a song should increase based on its
/// maximum quality.
let calculateQualityIncreaseOf (Unfinished(song, maxQuality, _)) =
    let improvementStep =
        match song.Length.Minutes with
        | minutes when minutes < 5<minute> -> 0.20
        | minutes when minutes < 10<minute> -> 0.15
        | minutes when minutes < 20<minute> -> 0.10
        | _ -> 0.08

    maxQuality
    |> float
    |> fun max -> max * improvementStep
    |> ceil
    |> int
    |> fun increase -> increase * 1<quality>

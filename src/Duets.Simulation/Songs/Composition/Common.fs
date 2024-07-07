module Duets.Simulation.Songs.Composition.Common

open Duets.Simulation.Queries
open Duets.Entities
open Duets.Entities.Skill

/// Computes the score associated with each member of the band for the song.
let qualityForMember state genre (currentMember: CurrentMember) =
    let genreSkill = create <| SkillId.Genre genre

    let influencingSkills =
        [ SkillId.Composition
          SkillId.Instrument currentMember.Role
          genreSkill.Id ]

    influencingSkills
    |> List.map (Skills.characterSkillWithLevel state currentMember.CharacterId)
    |> List.map snd
    |> List.sum
    |> fun total -> total / influencingSkills.Length

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

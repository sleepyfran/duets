module Simulation.Songs.Composition.Common

open Aether
open Simulation.Queries
open Entities
open Entities.Skill

/// Computes the score associated with each member of the band for the song.
let qualityForMember state genre (currentMember: CurrentMember) =
    let genreSkill = create <| Genre(genre)

    let influencingSkills =
        [ Composition
          (Instrument
           <| Instrument.createInstrument currentMember.Role)
          genreSkill.Id ]

    influencingSkills
    |> List.map (Skills.characterSkillWithLevel state currentMember.Character.Id)
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
    |> fun score ->
        match score with
        | score when score < 5 -> 5<quality>
        | _ -> score * 1<quality>

/// Calculates at what rate the score of a song should increase based on its
/// maximum quality.
let calculateQualityIncreaseOf (maximum: MaxQuality) =
    maximum
    |> float
    |> fun max -> max * 0.20
    |> ceil
    |> int
    |> fun increase -> increase * 1<quality>

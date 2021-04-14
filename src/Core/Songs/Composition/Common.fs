module Core.Songs.Composition.Common

open Entities.Band
open Entities.Character
open Entities.Skill
open Mediator.Query
open Mediator.Queries.Core

/// Computes the score associated with each member of the band for the song.
let scoreForMember genre ((character: Character), role, _) =
  let genreSkill = createSkill <| Genre(genre)

  let influencingSkills =
    [ Composition
      (skillIdForRole role)
      genreSkill.Id ]

  influencingSkills
  |> List.map
       (fun skillId -> query (CharacterSkillLevelQuery(character.Id, skillId)))
  |> List.map snd
  |> List.sum
  |> fun total -> total / influencingSkills.Length

/// Computes the maximum score that the band can achieve for a song in a moment
/// in time given each member's skills in composition, the current genre and
/// the member's instrument skill.
let scoreForBand band =
  band.Members
  |> List.map (scoreForMember band.Genre)
  |> List.sum

/// Calculates at what rate the score of a song should increase based on its
/// maximum quality.
let calculateScoreIncreaseOf maximum =
  maximum / 20
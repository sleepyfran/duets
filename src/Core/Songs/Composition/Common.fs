module Core.Songs.Composition.Common

open Core.Skills.Queries
open Core.Songs.Queries
open Entities.Band
open Entities.Character
open Entities.Skill
open Storage.State

/// Computes the score associated with each member of the band for the song.
let qualityForMember genre ((character: Character), role, _) =
  let genreSkill = createSkill <| Genre(genre)

  let influencingSkills =
    [ Composition
      (skillIdForRole role)
      genreSkill.Id ]

  influencingSkills
  |> List.map (characterSkillWithLevel character.Id)
  |> List.map snd
  |> List.sum
  |> fun total -> total / influencingSkills.Length

/// Computes the maximum score that the band can achieve for a song in a moment
/// in time given each member's skills in composition, the current genre and
/// the member's instrument skill.
let qualityForBand band =
  band.Members
  |> List.map (qualityForMember band.Genre)
  |> List.sum
  |> fun score ->
       match score with
       | score when score < 5 -> 5
       | _ -> score

/// Calculates at what rate the score of a song should increase based on its
/// maximum quality.
let calculateQualityIncreaseOf maximum =
  maximum
  |> float
  |> fun max -> max * 0.20
  |> ceil
  |> int

/// Adds or modifies a given unfinished song into the given band's repertoire.
let addUnfinishedSong songWithQualities (band: Band) =
  let allUnfinishedSongs = unfinishedSongs ()

  unfinishedSongsByBand band.Id
  |> List.append [ songWithQualities ]
  |> fun updatedSongs -> Map.add band.Id updatedSongs allUnfinishedSongs
  |> fun updatedSongs ->
       modifyState
         (fun state ->
           { state with
               UnfinishedSongs = updatedSongs })

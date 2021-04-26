module Simulation.Songs.Composition.Common

open Simulation.Skills.Queries
open Simulation.Songs.Queries
open Entities
open Entities.Skill
open Storage.State

/// Computes the score associated with each member of the band for the song.
let qualityForMember genre ((character: Character), role, _) =
  let genreSkill = create <| Genre(genre)

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

/// Adds or modifies a given unfinished song into the given band's repertoire.
let addUnfinishedSong songWithQualities (band: Band) =
  let (UnfinishedSong (song), _, _) = songWithQualities

  unfinishedSongsByBand band.Id
  |> Map.add song.Id songWithQualities
  |> fun updatedSongs -> Map.add band.Id updatedSongs (unfinishedSongs ())
  |> fun updatedSongs ->
       modifyState
         (fun state ->
           { state with
               BandRepertoire =
                 { state.BandRepertoire with
                     Unfinished = updatedSongs } })

module Core.Songs.Composition.ComposeSong

open Entities.Band
open Entities.Character
open Entities.State
open Entities.Skill
open Entities.Song
open Mediator.Mutations.Storage
open Mediator.Mutation
open Mediator.Queries.Core
open Mediator.Queries.Storage
open Mediator.Query

/// Computes the score associated with each member of the band for the song.
let private scoreForMember genre ((character: Character), role, _) =
  let genreSkill = createSkill <| Genre(genre)

  let influencingSkills =
    [ Composition
      (skillIdForRole role)
      genreSkill.Id ]

  influencingSkills
  |> List.map
       (fun skillId -> query (CharacterSkillLevel(character.Id, skillId)))
  |> List.map snd
  |> List.sum
  |> fun total -> total / influencingSkills.Length

/// Computes the maximum score that the band can achieve for a song in a moment
/// in time given each member's skills in composition, the current genre and
/// the member's instrument skill.
let private scoreForBand band =
  band.Members
  |> List.map (scoreForMember band.Genre)
  |> List.sum

/// Orchestrates the song composition, which calculates the qualities of a song
/// and adds them with the song to the band's unfinished songs.
let composeSong input =
  let band = query BandQuery
  let maximumScore = scoreForBand band
  let initialScore = maximumScore / 20 * 100
  let unfinishedSongs = query UnfinishedSongsQuery

  let songWithQuality =
    (UnfinishedSong input, MaxQuality maximumScore, Quality initialScore)

  let unfinishedSongsByBand =
    Map.tryFind band.Id unfinishedSongs
    |> Option.defaultValue []

  let unfinishedWithSong =
    unfinishedSongsByBand
    @ [ UnfinishedWithQualities songWithQuality ]

  mutate (
    ModifyStateMutation
      (fun state ->
        { state with
            UnfinishedSongs =
              Map.add state.Band.Id unfinishedWithSong state.UnfinishedSongs })
  )

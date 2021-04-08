module Core.Resolvers.Songs.Composition.Mutations

open Entities.Band
open Entities.Character
open Entities.State
open Entities.Skill
open Entities.Song
open Mediator.Mutations.Storage
open Mediator.Mutations.Types
open Mediator.Queries.Types
open Mediator.Queries.Core

/// Computes the score associated with each member of the band for the song.
let scoreForMember (query: QueryFn<_, _>)
                   genre
                   ((character: Character), role, _)
                   =
  let genreSkill = createSkill <| Genre(genre)

  let influencingSkills =
    [ Composition
      (skillIdForRole role)
      genreSkill.Id ]

  influencingSkills
  |> List.map (fun skillId ->
       query
         (CharacterSkillLevel
           { CharacterId = character.Id
             SkillId = skillId }))
  |> List.map snd
  |> List.sum
  |> fun total -> total / influencingSkills.Length

/// Computes the maximum score that the band can achieve for a song in a moment
/// in time given each member's skills in composition, the current genre and
/// the member's instrument skill.
let scoreForBand query band =
  band.Members
  |> List.map (scoreForMember query band.Genre)
  |> List.sum

/// Orchestrates the song composition, which calculates the qualities of a song
/// and adds them with the song to the band's unfinished songs.
let composeSong (query: QueryFn<_, _>) (mutate: MutationFn<_, _>) input =
  mutate
    (ModifyStateMutation(fun state ->
      let maximumScore = scoreForBand query state.Band
      let initialScore = maximumScore / 20 * 100
      
      let songWithQuality =
        (UnfinishedSong input, MaxQuality maximumScore, Quality initialScore)

      let unfinishedSongsByBand =
        Map.tryFind state.Band.Id state.UnfinishedSongs
        |> Option.defaultValue []

      let unfinishedWithSong =
        unfinishedSongsByBand
        @ [ UnfinishedWithQualities songWithQuality ]

      { state with
          UnfinishedSongs =
            Map.add state.Band.Id unfinishedWithSong state.UnfinishedSongs }))

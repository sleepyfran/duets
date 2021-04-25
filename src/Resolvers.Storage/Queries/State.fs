module Resolvers.Storage.Queries.State

open Entities.Song
open Mediator.Queries.Types
open Mediator.Registries
open Storage.State

let stateProperty accessor _ = getState () |> accessor |> box

let bandAccessor =
  stateProperty (fun state -> state.Bands.Head)

let characterAccessor =
  stateProperty (fun state -> state.Character)

let characterSkillsAccessor =
  stateProperty (fun state -> state.CharacterSkills)

let unfinishedSongsAccessor =
  stateProperty (fun state -> state.UnfinishedSongs)

let unfinishedSongsByBand bandId =
  getState ()
  |> fun state -> state.UnfinishedSongs
  |> Map.tryFind bandId
  |> Option.defaultValue []

let unfinishedSongsByBandAccessor bandId =
  bandId |> unfinishedSongsByBand |> box

let unfinishedSongAccessor bandId songId =
  bandId
  |> unfinishedSongsByBand
  |> List.tryFind (fun ((UnfinishedSong (song)), _, _) -> song.Id = songId)
  |> box

let register () =
  Registries.QueryRegistry.AddHandler QueryId.Band bandAccessor
  Registries.QueryRegistry.AddHandler QueryId.Character characterAccessor

  Registries.QueryRegistry.AddHandler
    QueryId.CharacterSkills
    characterSkillsAccessor

  Registries.QueryRegistry.AddHandler
    QueryId.UnfinishedSongs
    unfinishedSongsAccessor

  Registries.QueryRegistry.AddHandler
    QueryId.UnfinishedSongsByBand
    (fun bandId -> unbox bandId |> unfinishedSongsByBandAccessor)

  Registries.QueryRegistry.AddHandler
    QueryId.UnfinishedSongById
    (fun idTuple ->
      idTuple
      |> unbox
      |> fun (bandId, songId) -> unfinishedSongAccessor bandId songId)

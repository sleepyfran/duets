module Resolvers.Storage.Queries.State

open Mediator.Queries.Types
open Mediator.Registries
open Resolvers.Common
open Storage.State

let stateProperty accessor _ = getState () |> accessor |> box

let bandAccessor = stateProperty (fun state -> state.Band)

let characterAccessor =
  stateProperty (fun state -> state.Character)

let characterSkillsAccessor =
  stateProperty (fun state -> state.CharacterSkills)

let unfinishedSongsAccessor =
  stateProperty (fun state -> state.UnfinishedSongs)

let register () =
  Registries.QueryRegistry.AddHandler QueryId.Band bandAccessor
  Registries.QueryRegistry.AddHandler QueryId.Character characterAccessor

  Registries.QueryRegistry.AddHandler
    QueryId.CharacterSkills
    characterSkillsAccessor

  Registries.QueryRegistry.AddHandler
    QueryId.UnfinishedSongs
    unfinishedSongsAccessor

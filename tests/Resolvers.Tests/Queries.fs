module Resolvers.Tests

open Entities.Band
open Entities.Song
open Test.Common
open Entities
open Entities.Character
open Entities.Skill
open Mediator.Query
open Mediator.Queries.Core
open Mediator.Queries.Storage
open NUnit.Framework
open FsUnit

let queries =
  [ CharacterSkillLevelQuery
    <| (CharacterId <| Identity.create (), Composition)
    |> box
    SavegameStateQuery |> box
    CurrentBandQuery |> box
    CharacterQuery |> box
    CharacterSkillsQuery |> box
    UnfinishedSongsQuery |> box
    RolesQuery |> box
    GenresQuery |> box
    VocalStylesQuery |> box ]

let nullCheck (result: 'a) = should not' (be null) result

[<SetUp>]
let Setup () = initStateWithDummies ()

[<Test>]
let QueriesShouldSuccessfullyExecute () =
  CharacterSkillLevelQuery(CharacterId <| Identity.create (), Composition)
  |> query
  |> nullCheck

  SavegameStateQuery |> query |> nullCheck
  CurrentBandQuery |> query |> nullCheck
  CharacterQuery |> query |> nullCheck
  CharacterSkillsQuery |> query |> nullCheck
  UnfinishedSongsQuery |> query |> nullCheck

  UnfinishedSongByBandQuery(BandId(Identity.create ()))
  |> query
  |> nullCheck

  UnfinishedSongByIdQuery
    (BandId(Identity.create ()))
    (SongId(Identity.create ()))
  |> query
  |> ignore

  RolesQuery |> query |> nullCheck
  GenresQuery |> query |> nullCheck
  VocalStylesQuery |> query |> nullCheck

  Assert.Pass()

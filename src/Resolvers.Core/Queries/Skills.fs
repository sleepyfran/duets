module Resolvers.Core.Queries.CharacterSkillWithLevelQuery

open Core.Skills
open Mediator.Registries
open Mediator.Queries.Types
open Resolvers.Common

let register () =
  Registries.QueryRegistry.AddHandler
    QueryId.CharacterSkillLevel
    (boxed getCharacterSkillWithLevel)

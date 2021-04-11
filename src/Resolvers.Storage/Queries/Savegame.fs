module Resolvers.Storage.Queries.Savegame

open Mediator.Registries
open Mediator.Queries.Types
open Storage.Savegame
open Resolvers.Common

let register () =
  Registries.QueryRegistry.AddHandler
    QueryId.SavegameState
    (boxed savegameState)

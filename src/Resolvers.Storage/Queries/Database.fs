module Resolvers.Storage.Queries.Database

open Mediator.Queries.Types
open Mediator.Registries
open Storage.Database
open Resolvers.Common

let register () =
  Registries.QueryRegistry.AddHandler QueryId.Roles (boxed roles)
  Registries.QueryRegistry.AddHandler QueryId.Genres (boxed genres)
  Registries.QueryRegistry.AddHandler QueryId.VocalStyle (boxed vocalStyles)

module Mediator.QueryHub

open Mediator.Queries
open Mediator.Queries.Types

/// Defines the handlers for each query.
let handlers id =
  match id with
  | QueryId.Roles -> Database.Resolvers.roles
  | QueryId.Genres -> Database.Resolvers.genres
  | QueryId.SavegameState -> Storage.Resolvers.savegameState
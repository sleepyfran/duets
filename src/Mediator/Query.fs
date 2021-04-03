module Mediator.Query

open Mediator.Queries.Types

/// Queries the given query definition and return its result.
let query<'Parameter, 'Result> (definition: Query<'Parameter, 'Result>): 'Result =
  match definition.Id with
  | QueryId.SavegameState -> unbox Storage.Resolvers.savegameState ()
  | QueryId.Genres -> unbox Database.Resolvers.genres ()
  | QueryId.Roles -> unbox Database.Resolvers.roles ()

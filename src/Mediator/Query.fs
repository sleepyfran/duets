module Mediator.Query

open Mediator.Queries.Types

/// Queries the given query definition and return its result.
let query<'Parameter, 'Result> (definition: Query<'Parameter, 'Result>): 'Result =
  match definition.Id with
  | QueryId.GetState -> unbox Storage.Resolvers.State.getState ()
  | QueryId.SavegameState -> unbox Storage.Resolvers.Savegame.savegameState ()
  | QueryId.Genres -> unbox Database.Resolvers.Database.genres ()
  | QueryId.Roles -> unbox Database.Resolvers.Database.roles ()

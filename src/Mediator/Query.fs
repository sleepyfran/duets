module Mediator.Query

open Mediator.Queries.Types

/// Queries the given query definition and return its result.
let rec query<'Parameter, 'Result> : QueryFn<'Parameter, 'Result> =
  fun definition ->
    // TODO: Make section in between Option.get and unbox less ugly.
    match definition.Id with
    | QueryId.GetState -> unbox Storage.Resolvers.State.getState ()
    | QueryId.SavegameState -> unbox Storage.Resolvers.Savegame.savegameState ()
    | QueryId.Genres -> unbox Database.Resolvers.Database.genres ()
    | QueryId.Roles -> unbox Database.Resolvers.Database.roles ()
    | QueryId.VocalStyle -> unbox Database.Resolvers.Database.vocalStyles ()
    | QueryId.CharacterSkillLevel ->
        Option.get definition.Parameter
        |> unbox
        |> Core.Resolvers.Skills.Queries.getCharacterSkillWithLevel query
        |> unbox

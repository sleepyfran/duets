module Mediator.Queries.Storage

open Mediator.Queries.Types

type SavegameState = NotAvailable | Available

/// Retrieves whether there's a savegame available for loading or not.
let SavegameStateQuery = Query<SavegameState, unit>.WithoutParameter {
  Id = QueryId.SavegameState
}

type Role = string
type Genre = string

/// Retrieves the static list of roles available in the game.
let RolesQuery = Query<Role list, unit>.WithoutParameter {
  Id = QueryId.Roles
}

/// Retrieves the static list of genres available in the game.
let GenresQuery = Query<Genre list, unit>.WithoutParameter {
  Id = QueryId.Genres
}
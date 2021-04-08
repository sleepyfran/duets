module Mediator.Queries.Storage

open Entities.State
open Mediator.Queries.Types

/// Retrieves the current game state.
let GetStateQuery: Query<unit, State> =
  { Id = QueryId.GetState
    Parameter = None }

type SavegameState =
  | NotAvailable
  | Available

/// Retrieves whether there's a savegame available for loading or not.
let SavegameStateQuery: Query<unit, SavegameState> =
  { Id = QueryId.SavegameState
    Parameter = None }

type Role = string
type Genre = string
type VocalStyle = string

/// Retrieves the static list of roles available in the game.
let RolesQuery: Query<unit, Role list> =
  { Id = QueryId.Roles; Parameter = None }

/// Retrieves the static list of genres available in the game.
let GenresQuery: Query<unit, Genre list> =
  { Id = QueryId.Genres
    Parameter = None }

let VocalStylesQuery: Query<unit, VocalStyle list> =
  { Id = QueryId.VocalStyle
    Parameter = None }

module Mediator.Queries.Types

/// Contains all the IDs that can map to one query.
type QueryId =
  | Band
  | Character
  | CharacterSkills
  | UnfinishedSongs
  | SavegameState
  | Roles
  | Genres
  | VocalStyle
  | CharacterSkillLevel

/// Defines a query that can optionally take a parameter and returns a result.
type Query<'Parameter, 'Result> =
  { Id: QueryId
    Parameter: 'Parameter option }

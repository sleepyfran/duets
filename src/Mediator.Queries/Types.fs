module Mediator.Queries.Types

/// Contains all the IDs that can map to one query.
type QueryId = SavegameState | Roles | Genres

/// Defines a query that takes no parameters and returns a Result type.
type NonParameterizedQuery<'Result> = {
  Id: QueryId
}

/// Defines a query that takes a Parameter and returns a Result type.
type ParameterizedQuery<'Parameter, 'Result> = {
  Id: QueryId
  Parameter: 'Parameter
}

type Query<'Parameter, 'Result> =
  | WithParameter of ParameterizedQuery<'Parameter, 'Result>
  | WithoutParameter of NonParameterizedQuery<'Result>

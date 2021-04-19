module Mediator.Query

open Mediator.Queries.Types
open Mediator.Registries

/// Queries the given query definition and return its result.
let rec query<'Parameter, 'Result>
  (query: Query<'Parameter, 'Result>)
  : 'Result =
  Registries.QueryRegistry.GetHandler query.Id
  |> fun handler ->
       match query.Parameter with
       | Some param -> handler param
       | None -> handler ()
  |> unbox

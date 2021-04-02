module Mediator.Query

open Mediator.Queries.Types

/// Queries the given query definition and return its result.
let query definition =
  match definition with
  | WithParameter _ -> raise (System.NotImplementedException())
  | WithoutParameter query ->
      let handler = handlers.[query.Id]
      handler ()
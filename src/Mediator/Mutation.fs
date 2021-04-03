module Mediator.Mutation

open Mediator.Mutations.Types

/// Executes the mutation specified in definition.
let mutate<'Parameter, 'Result> (definition: Mutation<'Parameter, 'Result>)
                                  : 'Result =
  match definition.Id with
  | MutationId.StartGame -> raise (System.NotImplementedException())

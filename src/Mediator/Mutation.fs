module Mediator.Mutation

open Mediator.MutationHub
open Mediator.Mutations.Types

/// Executes the mutation specified in definition.
let mutation definition =
  match definition with
  | WithParameter mutation ->
      let handler = handlers.[mutation.Id]
      handler mutation.Parameter
  | WithoutParameter mutation ->
      let handler = handlers.[mutation.Id]
      handler ()
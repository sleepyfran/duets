module Mediator.Mutation

open Mediator.Mutations.Types
open Mediator.Registries

/// Executes the mutation specified in definition and optionally returns a result.
let rec mutate<'Parameter, 'Result>
  (mutation: Mutation<'Parameter, 'Result>)
  : 'Result =
  Registries.MutationRegistry.GetHandler mutation.Id
  |> fun handler ->
       match mutation.Parameter with
       | Some param -> handler param
       | None -> handler ()
  |> unbox

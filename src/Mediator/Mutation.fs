module Mediator.Mutation

open Core.Resolvers
open Mediator.Mutations.Types
open Mediator.Query

/// Executes the mutation specified in definition.
let rec mutate<'Parameter, 'Result> (definition: Mutation<'Parameter, 'Result>)
                                    : 'Result =
  // TODO: Make section in between Option.get and unbox less ugly.
  match definition.Id with
  | MutationId.StartGame ->
      Option.get definition.Parameter
      |> unbox
      |> Setup.Transformations.toValidatedStartGameMutation
      |> Result.map (Setup.Mutations.startGame query mutate)
      |> unbox
  | MutationId.SetState ->
      Option.get definition.Parameter
      |> unbox
      |> Storage.Resolvers.State.setState query mutate
      |> unbox

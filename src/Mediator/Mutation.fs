module Mediator.Mutation

open Core.Resolvers
open Mediator.Mutations.Types
open Mediator.Query

/// Executes the mutation specified in definition.
let rec mutate<'Parameter, 'Result> : MutationFn<'Parameter, 'Result> =
  fun definition ->
    // TODO: Make section in between Option.get and unbox less ugly.
    match definition.Id with
    | StartGame ->
        Option.get definition.Parameter
        |> unbox
        |> Setup.Transformations.toValidatedStartGameMutation
        |> Result.map (Setup.Mutations.startGame query mutate)
        |> unbox
    | SetState ->
        Option.get definition.Parameter
        |> unbox
        |> Storage.Resolvers.State.setState query mutate
        |> unbox
    | ModifyState ->
        Option.get definition.Parameter
        |> unbox
        |> Storage.Resolvers.State.modifyState query mutate
        |> unbox
    | ComposeSong ->
        Option.get definition.Parameter
        |> unbox
        |> Songs.Composition.Transformations.toValidatedSong
        |> Result.map (Songs.Composition.Mutations.composeSong query mutate)
        |> unbox

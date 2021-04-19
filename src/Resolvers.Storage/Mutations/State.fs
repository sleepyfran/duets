module Resolvers.Storage.Mutations.State

open Mediator.Mutations.Types
open Mediator.Registries
open Resolvers.Common
open Storage.State

let register () =
  Registries.MutationRegistry.AddHandler MutationId.SetState (boxed setState)

  Registries.MutationRegistry.AddHandler
    MutationId.ModifyState
    (boxed modifyState)

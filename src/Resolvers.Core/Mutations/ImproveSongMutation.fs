module Resolvers.Core.Mutations.ImproveSong

open Core.Songs.Composition.ImproveSong
open Mediator.Mutations.Types
open Mediator.Registries
open Resolvers.Common

let register () =
  Registries.MutationRegistry.AddHandler
    MutationId.ImproveSong
    (boxed improveSong)

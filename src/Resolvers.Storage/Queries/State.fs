module Resolvers.Storage.Queries.State

open Mediator.Queries.Types
open Mediator.Registries
open Resolvers.Common
open Storage.State

let register () =
  Registries.QueryRegistry.AddHandler QueryId.GetState (boxed getState)

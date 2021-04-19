module Mediator.Registries

open System.Threading
open Mediator.Registry
open Mediator.Mutations.Types
open Mediator.Queries.Types

type Registries private () =
  static let queryRegistryInstance = new ThreadLocal<_>(Registry<QueryId>)
  static let mutationRegistryInstance = new ThreadLocal<_>(Registry<MutationId>)

  static member QueryRegistry = queryRegistryInstance.Value

  static member MutationRegistry = mutationRegistryInstance.Value

module Mediator.Mutations.Storage

open Entities.State
open Mediator.Mutations.Types

type SetStateMutationInput = State

/// Sets the current value of the state.
let SetStateMutation state: Mutation<SetStateMutationInput, unit> =
  { Id = MutationId.SetState
    Parameter = Some state }

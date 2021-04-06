module Mediator.Mutations.Types

/// Contains all the IDs that can map to one mutation.
type MutationId =
  | StartGame
  | SetState
  | ModifyState
  | ComposeSong

/// Defines a mutation that can optionally take parameters and returns a result.
type Mutation<'Parameter, 'Result> =
  { Id: MutationId
    Parameter: 'Parameter option }

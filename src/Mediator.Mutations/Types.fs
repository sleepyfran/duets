module Mediator.Mutations.Types

/// Contains all the IDs that can map to one mutation.
type MutationId = StartGame

/// Defines a mutation that takes no parameters and returns a Result type.
type NonParameterizedMutation<'Result> = {
  Id: MutationId
}

/// Defines a mutation that takes a Parameter and returns a Result type.
type ParameterizedMutation<'Parameter, 'Result> = {
  Id: MutationId
  Parameter: 'Parameter
}

type Mutation<'Parameter, 'Result> =
  | WithParameter of ParameterizedMutation<'Parameter, 'Result>
  | WithoutParameter of NonParameterizedMutation<'Result>

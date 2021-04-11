module Resolvers.Common

/// Unboxes the given input into the 'Parameter type, calls the given handler
/// and then boxes the value back.
let boxed<'Parameter, 'Result> (handler: 'Parameter -> 'Result) input =
  unbox<'Parameter> input |> handler |> box

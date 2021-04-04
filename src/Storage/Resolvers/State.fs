module Storage.Resolvers.State

open Entities.State

/// Holds the current state. TODO: Change to something less smelly.
type StateWrapper =
  [<DefaultValue>]
  static val mutable private state: State

  static member State
    with get () = StateWrapper.state
    and set (value) = StateWrapper.state <- value

let setState _ _ input = StateWrapper.State <- input

let getState () = StateWrapper.State

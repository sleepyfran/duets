module Storage.State

open Entities.State

/// Holds the current state. TODO: Change to something less smelly.
type StateWrapper =
  [<DefaultValue>]
  static val mutable private state: State

  static member State
    with get () = StateWrapper.state
    and set (value) = StateWrapper.state <- value

let getState () = StateWrapper.State

let modifyState modify =
  StateWrapper.State <- modify StateWrapper.State

let setState input = StateWrapper.State <- input

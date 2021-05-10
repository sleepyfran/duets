module State.Tests.Common

open Test.Common

open Entities

/// Initializes the state to a dummy state.
let initState () = State.Root.set dummyState

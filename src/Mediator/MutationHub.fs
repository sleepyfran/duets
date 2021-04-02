module Mediator.MutationHub

open Mediator.Mutations.Types

/// Defines the handlers for each mutation ID. TODO: Add real handlers.
let handlers =
  Map [ (MutationId.StartGame, fun () -> ()) ]

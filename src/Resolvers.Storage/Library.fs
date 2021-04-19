module Resolvers.Storage.Setup

open Resolvers.Storage

let init () =
  // Initialize all queries.
  Queries.Savegame.register ()
  Queries.Database.register ()
  Queries.State.register ()

  // Initialize all mutations.
  Mutations.State.register ()

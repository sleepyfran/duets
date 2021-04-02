module Storage.Resolvers

open Mediator.Queries.Storage

/// Returns whether there's some savegame available or not.
let savegameState () = NotAvailable
module Data.Queries.Genres

open Entities

/// Retrieves all the genres available.
let getAll () : Genre list = [ "Rock"; "Metal" ]

module Data.Queries.Genres

open Entities.Genre

/// Retrieves all the genres available.
let getAll (): Genre [] = [| "Rock"; "Metal" |]

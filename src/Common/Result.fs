module Common.Result

open System

/// Returns the OK side of a result or raises an exception otherwise. Only use
/// when the result is KNOWN to only have an OK.
let unwrap result =
  match result with
  | Ok res -> res
  | Error _ -> raise (NullReferenceException())

/// Returns the Error side of a result or raises an exception otherwise.
let unwrapError result =
  match result with
  | Ok _ -> raise (NullReferenceException())
  | Error error -> error

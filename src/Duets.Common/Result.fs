module Duets.Common.Result

open System

/// Returns another result if the current one evaluates to Ok, otherwise
/// returns the error.
let andThen chainedResult = Result.bind (fun _ -> chainedResult)

/// Returns the given value if the result is Ok, otherwise returns the error.
let transform value = Result.bind (fun _ -> Ok value)

/// Returns a result of the inner type of the option if it's some, otherwise
/// returns the given error.
let ofOption opt err =
    match opt with
    | Some x -> Ok x
    | None -> Error err

/// Returns a result of the inner type of the option if it's some, otherwise
/// returns the given error.
let ofOption' err opt = ofOption opt err

/// Returns an empty ok if the given bool is true, otherwise returns the error.
let ofBool error bool = if bool then Ok() else Error error

/// Executes either okFn or errorFn based on the given result and returns
/// the result of each function.
let switch okFn errorFn value =
    match value with
    | Ok value -> okFn value
    | Error err -> errorFn err

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

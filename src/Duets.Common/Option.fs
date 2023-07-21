module Duets.Common.Option

/// Returns a unit option as some if the given boolean is true and None otherwise.
let ofBool res =
    match res with
    | true -> Some()
    | false -> None

/// Returns a non-empty list option as some if the given list is not empty and
/// None otherwise.
let ofList xs =
    match xs with
    | [] -> None
    | _ -> Some xs

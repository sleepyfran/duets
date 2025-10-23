module Duets.Common.Option

/// Returns the value of the given option if it is some and throws an exception
/// otherwise.
let value opt =
    match opt with
    | Some x -> x
    | None -> failwith "Option is None"

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

/// Taps into an option, applying the given function if there is some content
/// and returning the param untouched. Similar to an `iter` and `map` but without
/// changing the value of the content.
let tap func opt =
    match opt with
    | Some x ->
        func x
        Some(x)
    | None -> None

module Duets.Common.Option

/// Returns a unit option as some if the given boolean is true and None otherwise.
let ofBool res =
    match res with
    | true -> Some()
    | false -> None

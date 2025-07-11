module Duets.Common.Tuple

/// Creates a tuple of two given its arguments.
let two first second = (first, second)

/// Returns the first element of a tuple of three elements.
let fst3 (first, _, _) = first

/// Returns the second element of a tuple of three elements.
let snd3 (_, second, _) = second

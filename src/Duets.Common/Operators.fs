module Duets.Common.Operators

/// Returns the result of performing the exclusive between operation on the
/// given values.
let (><) x (min, max) =
    (x > min) && (x < max)

/// Returns the result of performing the inclusive between operation on the
/// given values.
let (>=<) x (min, max) =
    (x >= min) && (x <= max)

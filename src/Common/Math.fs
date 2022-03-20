module Common.Math

/// Wraps Math.Clamp, which returns a clamped value between the inclusive range
/// of min and max.
let clamp (min: int<_>) (max: int<_>) (value: int<_>) =
    if value < min then min
    else if value > max then max
    else value

/// Wraps Math.Clamp, which returns a clamped value between the inclusive range
/// of min and max.
let clampFloat (min: float) (max: float) (value: float) =
    System.Math.Clamp(value, min, max)

/// Rounds a float to its nearest int.
let roundToNearest (flt: float) = System.Math.Round(flt) |> int

/// Rounds a float to its upper nearest int.
let ceilToNearest (flt: float) =
    System.Math.Ceiling flt |> roundToNearest

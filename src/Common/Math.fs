module Common.Math

/// Wraps Math.Clamp, which returns a clamped value between the inclusive range
/// of min and max.
let clamp (min: int<_>) (max: int<_>) (value: int<_>) =
    if value < min then min
    else if value > max then max
    else value

/// Wraps clamp passing the value twice so that only the lower bound is clamped.
let lowerClamp (min: int<_>) (value: int<_>) = clamp min value value

/// Wraps Math.Clamp, which returns a clamped value between the inclusive range
/// of min and max.
let clampFloat (min: float) (max: float) (value: float) =
    System.Math.Clamp(value, min, max)

/// Rounds a float to its nearest int.
let roundToNearest (flt: float) = System.Math.Round(flt) |> int

/// Ceils a float to its upper nearest integer value.
let ceil (flt: float) = System.Math.Ceiling flt

/// Rounds a float to its upper nearest int.
let ceilToNearest (flt: float) = ceil flt |> roundToNearest

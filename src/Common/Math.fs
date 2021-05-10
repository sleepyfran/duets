module Common.Math

/// Wraps Math.Clamp, which returns a clamped value between the inclusive range
/// of min and max.
let clamp (min: int) (max: int) (value: int) =
    System.Math.Clamp(value, min, max)

/// Rounds a float to its nearest int.
let roundToNearest (flt: float) = System.Math.Round(flt) |> int

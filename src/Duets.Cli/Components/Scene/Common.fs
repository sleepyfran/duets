module Duets.Cli.Components.Scene.Common

open Duets.Entities

/// Information about the time and weather that affects how a scene is lit.
type Lighting =
    { DayMoment: DayMoment
      Weather: WeatherCondition }

/// Returns whether it's dark enough for lights to be on.
let isDark (lighting: Lighting) =
    match lighting.DayMoment with
    | Evening
    | Night
    | Midnight -> true
    | _ -> false

/// Returns whether it's night time, when the moon and stars are out.
let isNight (lighting: Lighting) =
    match lighting.DayMoment with
    | Night
    | Midnight -> true
    | _ -> false

/// Creates a stable seed from the given string. String.GetHashCode is
/// randomized per process, so we can't rely on it to keep places looking the
/// same between sessions.
let seedOf (text: string) =
    text
    |> Seq.fold
        (fun hash char -> (hash ^^^ uint32 char) * 16777619u)
        2166136261u
    |> int

/// Centers the text in the given width, padding it with spaces.
let center width (text: string) =
    let padding = max 0 (width - text.Length)
    let left = padding / 2
    (String.replicate left " " + text).PadRight(width)

/// Pads the text to the given width, truncating it with an ellipsis if it
/// doesn't fit.
let fit width (text: string) =
    if text.Length > width then
        text.Substring(0, width - 1) + "…"
    else
        text.PadRight(width)

/// Repeats the pattern until it fills exactly the given width.
let repeat width (pattern: string) =
    String.replicate (width / pattern.Length + 1) pattern
    |> fun text -> text.Substring(0, width)

/// Minimum width of the terminal to draw the art of a scene.
let compactWidth = 40

/// Returns the width at which scenes are drawn.
let sceneWidth terminalWidth = min terminalWidth 120

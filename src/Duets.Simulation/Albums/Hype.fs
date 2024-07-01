module Duets.Simulation.Albums.Hype

open Duets.Common
open Duets.Entities

/// Re-calculates the hype for the given album. Initially only decreases by 0.1
/// and keeps 0.1 as the lowest value possible. In the future this might take
/// into account the band's fame as well as other events such as ads, new
/// releases, etc.
let reduceDailyHype releasedAlbum =
    releasedAlbum.Hype - 0.1 |> Math.clampFloat 0.1 1.0

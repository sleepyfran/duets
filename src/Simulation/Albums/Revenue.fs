module Simulation.Albums.Revenue

open Common
open Entities

let defaultRevenuePerStream = 0.003

/// Calculates the daily revenue of the given album based on the streams that
/// were generated the previous day.
let albumRevenue (previousDayStreams: int) =
    float previousDayStreams * defaultRevenuePerStream
    |> Math.roundToNearest
    |> (*) 1<dd>

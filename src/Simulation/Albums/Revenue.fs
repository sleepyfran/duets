module Simulation.Albums.Revenue

open Common
open Entities
open Simulation.Constants

/// Calculates the daily revenue of the given album based on the streams that
/// were generated the previous day.
let albumRevenue (previousDayStreams: int) =
    float previousDayStreams * revenuePerStream
    |> Math.roundToNearest
    |> (*) 1<dd>

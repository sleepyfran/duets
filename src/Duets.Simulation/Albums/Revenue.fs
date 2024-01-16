module Duets.Simulation.Albums.Revenue

open Duets.Entities
open Duets.Simulation

/// Calculates the daily revenue of the given album based on the streams that
/// were generated the previous day.
let albumRevenue (previousDayStreams: int) =
    float previousDayStreams
    * Config.Revenue.revenuePerStream
    |> decimal
    |> Amount.fromDecimal

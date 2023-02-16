module Duets.Simulation.Albums.FanIncrease

open Duets.Common
open Duets.Simulation

/// Increases the number of fans for the day based on the previous' day non-fan
/// streams.
let calculateFanIncrease nonFanStreams =
    float nonFanStreams
    * Config.MusicSimulation.fanIncreasePercentage
    |> Math.ceilToNearest

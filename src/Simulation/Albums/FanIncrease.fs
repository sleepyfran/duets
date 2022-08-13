module Simulation.Albums.FanIncrease

open Common
open Simulation

/// Increases the number of fans for the day based on the previous' day non-fan
/// streams.
let calculateFanIncrease nonFanStreams =
    float nonFanStreams
    * Config.MusicSimulation.fanIncreasePercentage
    |> Math.ceilToNearest

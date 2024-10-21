module Duets.Simulation.Albums.FanIncrease

open Duets.Entities
open Duets.Common
open Duets.Simulation

/// Increases the number of fans for the day based on the previous' day non-fan
/// streams.
let calculateFanIncrease nonFanStreams =
    float nonFanStreams * Config.MusicSimulation.fanIncreasePercentage
    |> Math.ceilToNearest
    |> (*) 1<fans>

/// Applies the given fan increase to all cities in the fan base.
let applyFanIncrease fanIncrease (currentFans: FanBaseByCity) =
    currentFans |> Map.map (fun _ fans -> fans + fanIncrease)

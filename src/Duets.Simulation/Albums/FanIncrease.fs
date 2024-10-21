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
let applyFanIncrease band fanIncrease =
    // Ensure that we always have at least one city in the fan base, otherwise
    // it will be impossible to ever increase the number of fans.
    let fanBase =
        if band.Fans |> Map.isEmpty then
            [ band.OriginCity, 0<fans> ] |> Map.ofList
        else
            band.Fans

    fanBase |> Map.map (fun _ fans -> fans + fanIncrease)

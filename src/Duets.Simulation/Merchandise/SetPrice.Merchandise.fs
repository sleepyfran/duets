module Duets.Simulation.Merchandise.SetPrice

open Duets.Entities

/// Sets the price of the given item for the current band.
let setPrice band item price =
    MerchPriceSet(band, item, price) |> List.singleton

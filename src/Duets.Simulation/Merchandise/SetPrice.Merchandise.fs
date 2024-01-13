module Duets.Simulation.Merchandise.SetPrice

open Duets.Entities

type MerchPriceError = InvalidPrice

/// Sets the price of the given item for the current band, if the price is valid.
let setPrice band item price =
    if price <= 1m<dd> || price > 1000m<dd> then
        Error InvalidPrice
    else
        MerchPriceSet(band, item, price) |> Ok

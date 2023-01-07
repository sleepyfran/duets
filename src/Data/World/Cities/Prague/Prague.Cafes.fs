module Data.World.Cities.Prague.Cafes

open Entities
open Data.Items.Drink
open Data.World

let addMamaCoffee zone =
    let shop =
        { AvailableItems = Coffee.all
          PriceModifier = 2<multiplier> }

    World.Place.create
        ("1e9de63f-c248-4b35-b964-676f8139fa94" |> Identity.from)
        "Mama Coffee"
        85<quality>
        (Cafe shop)
        zone
    |> World.Place.changeOpeningHours Everywhere.Common.cafeOpeningHours
    |> World.City.addPlace

let addTheMiners zone =
    let shop =
        { AvailableItems = Coffee.all
          PriceModifier = 2<multiplier> }

    World.Place.create
        ("ce9a608c-8e39-4456-9c10-fc48d8f25fc6" |> Identity.from)
        "The Miners"
        75<quality>
        (Cafe shop)
        zone
    |> World.Place.changeOpeningHours Everywhere.Common.cafeOpeningHours
    |> World.City.addPlace

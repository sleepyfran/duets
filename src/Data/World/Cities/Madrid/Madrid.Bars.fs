module Data.World.Cities.Madrid.Bars

open Entities
open Data.World

let addWrongWay zone =
    let shop =
        { AvailableItems = CommonItems.pubDrinks @ CommonItems.pubFood
          PriceModifier = 3<multiplier> }

    World.Place.create
        ("1085e199-f5ad-4011-bc7e-4c55dbd4a25e" |> Identity.from)
        "Wrong Way"
        70<quality>
        (Bar shop)
        zone
    |> World.Place.changeOpeningHours Everywhere.Common.barOpeningHours
    |> World.City.addPlace

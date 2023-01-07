module Data.World.Cities.Prague.Bars

open Entities
open Data.World

let addBeerGeek zone =
    let shop =
        { AvailableItems = CommonItems.pubDrinks @ CommonItems.pubFood
          PriceModifier = 3<multiplier> }

    World.Place.create
        ("570c6572-22b6-43e8-a162-5ba44a00a489" |> Identity.from)
        "Beer Geek"
        80<quality>
        (Bar shop)
        zone
    |> World.Place.changeOpeningHours Everywhere.Common.barOpeningHours
    |> World.City.addPlace

let addPubble zone =
    let shop =
        { AvailableItems = CommonItems.pubDrinks @ CommonItems.pubFood
          PriceModifier = 2<multiplier> }

    World.Place.create
        ("a1fe2d4d-e16e-4204-ba6f-0675c9defa19" |> Identity.from)
        "Pubble"
        85<quality>
        (Bar shop)
        zone
    |> World.Place.changeOpeningHours Everywhere.Common.barOpeningHours
    |> World.City.addPlace

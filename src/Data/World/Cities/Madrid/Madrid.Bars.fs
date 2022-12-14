module Data.World.Cities.Madrid.Bars

open Entities

let addWrongWay zone =
    let shop =
        { AvailableItems = CommonItems.pubDrinks @ CommonItems.pubFood
          PriceModifier = 3<multiplier> }

    { Id = "1085e199-f5ad-4011-bc7e-4c55dbd4a25e" |> Identity.from |> PlaceId
      Name = "Wrong Way"
      Quality = 70<quality>
      Type = Bar shop
      Zone = zone }
    |> World.City.addPlace

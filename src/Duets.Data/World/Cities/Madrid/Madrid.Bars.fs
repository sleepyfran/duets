module Duets.Data.World.Cities.Madrid.Bars

open Duets.Entities
open Duets.Data.World

let addWrongWay zone =
    let shop =
        { AvailableItems = CityCommonItems.beers @ CityCommonItems.pubFood
          PriceModifier = 3<multiplier> }

    let bar = World.Node.create 0 RoomType.Bar

    let roomGraph = World.Graph.from bar

    World.Place.create
        ("1085e199-f5ad-4011-bc7e-4c55dbd4a25e" |> Identity.from)
        "Wrong Way"
        70<quality>
        (Bar shop)
        roomGraph
        zone
    |> World.Place.changeOpeningHours Everywhere.Common.barOpeningHours
    |> World.City.addPlace

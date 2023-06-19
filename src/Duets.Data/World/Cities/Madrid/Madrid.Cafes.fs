module Duets.Data.World.Cities.Madrid.Cafes

open Duets.Entities
open Duets.Data.World

let addAmbuCoffee zone =
    let shop =
        { AvailableItems = Everywhere.Common.coffeeShopItems
          PriceModifier = 2<multiplier> }

    let cafe = World.Node.create 0 RoomType.Cafe

    let roomGraph = World.Graph.from cafe

    World.Place.create
        ("edaf78ca-c2b9-460b-b9e0-c2dcd9e64922" |> Identity.from)
        "Ambu Coffee"
        90<quality>
        (Cafe shop)
        roomGraph
        zone
    |> World.Place.changeOpeningHours Everywhere.Common.cafeOpeningHours
    |> World.City.addPlace

let addMisionCafe zone =
    let shop =
        { AvailableItems = Everywhere.Common.coffeeShopItems
          PriceModifier = 2<multiplier> }

    let cafe = World.Node.create 0 RoomType.Cafe

    let roomGraph = World.Graph.from cafe

    World.Place.create
        ("4c5988c1-90cf-4ea0-9336-bb9a38836eba" |> Identity.from)
        "Misión Cafe"
        85<quality>
        (Cafe shop)
        roomGraph
        zone
    |> World.Place.changeOpeningHours Everywhere.Common.cafeOpeningHours
    |> World.City.addPlace

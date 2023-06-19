module Duets.Data.World.Cities.Prague.Restaurants

open Duets.Entities
open Duets.Data.Items.Food
open Duets.Data.Items.Drink
open Duets.Data.World

let addChilliAndLime zone =
    let food =
        [ JapaneseFood.gyoza
          VietnameseFood.bunBoNamBo
          VietnameseFood.nemCuonBo
          VietnameseFood.nemCuonTom
          VietnameseFood.phoBo ]

    let drinks =
        [ Beer.saigonBottle
          Beer.singhaBottle
          SoftDrinks.cocaColaBottle
          SoftDrinks.homemadeLemonade ]

    let shop =
        { AvailableItems = food @ drinks
          PriceModifier = 2<multiplier> }

    let restaurant = World.Node.create 0 RoomType.Restaurant

    let roomGraph = World.Graph.from restaurant

    World.Place.create
        ("23b3e34e-25d2-4bd0-ac15-26a7acf45e01" |> Identity.from)
        "Chilli and Lime"
        85<quality>
        (Restaurant shop)
        roomGraph
        zone
    |> World.Place.changeOpeningHours Everywhere.Common.restaurantOpeningHours
    |> World.City.addPlace

let addTaiko zone =
    let food =
        [ JapaneseFood.gyoza
          JapaneseFood.misoRamen
          JapaneseFood.tonkotsuRamen
          JapaneseFood.wakame ]

    let drinks =
        [ Beer.sapporoBottle
          SoftDrinks.homemadeLemonade
          SoftDrinks.cocaColaBottle ]

    let shop =
        { AvailableItems = drinks @ food
          PriceModifier = 2<multiplier> }

    let restaurant = World.Node.create 0 RoomType.Restaurant

    let roomGraph = World.Graph.from restaurant

    World.Place.create
        ("5fe0efa6-b0b1-4417-ba0b-a3fb256c5464" |> Identity.from)
        "TAIKO"
        90<quality>
        (Restaurant shop)
        roomGraph
        zone
    |> World.Place.changeOpeningHours Everywhere.Common.restaurantOpeningHours
    |> World.City.addPlace

let addForkys zone =
    let food = VegetarianFood.all

    let drinks = SoftDrinks.all

    let shop =
        { AvailableItems = drinks @ food
          PriceModifier = 1<multiplier> }

    let restaurant = World.Node.create 0 RoomType.Restaurant

    let roomGraph = World.Graph.from restaurant

    World.Place.create
        ("1c10d339-892e-4870-81f3-8f3f11d6b3a3" |> Identity.from)
        "Forky's"
        81<quality>
        (Restaurant shop)
        roomGraph
        zone
    |> World.Place.changeOpeningHours Everywhere.Common.restaurantOpeningHours
    |> World.City.addPlace

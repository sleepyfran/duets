module Duets.Data.World.Cities.Madrid.Restaurants

open Duets.Entities
open Duets.Data.Items.Food
open Duets.Data.Items.Drink
open Duets.Data.World

let addSumo zone =
    let food = JapaneseFood.all

    let drinks =
        [ Beer.saigonBottle
          Beer.singhaBottle
          SoftDrinks.cocaColaBottle
          SoftDrinks.homemadeLemonade ]
        @ CityCommonItems.beers

    let shop =
        { AvailableItems = food @ drinks
          PriceModifier = 2<multiplier> }

    let restaurant = World.Node.create 0 RoomType.Restaurant

    let roomGraph = World.Graph.from restaurant

    World.Place.create
        ("3b2e4bae-bcc5-49ab-af49-233a3375d157" |> Identity.from)
        "SUMO"
        88<quality>
        (Restaurant shop)
        roomGraph
        zone
    |> World.Place.changeOpeningHours Everywhere.Common.restaurantOpeningHours
    |> World.City.addPlace

let addHonestGreens zone =
    let food = MeatFood.all @ VegetarianFood.all

    let drinks = CityCommonItems.beers @ SoftDrinks.all

    let shop =
        { AvailableItems = food @ drinks
          PriceModifier = 2<multiplier> }

    let restaurant = World.Node.create 0 RoomType.Restaurant

    let roomGraph = World.Graph.from restaurant

    World.Place.create
        ("05711260-f915-4f0a-8209-3599b8780f4f" |> Identity.from)
        "Honest Greens"
        92<quality>
        (Restaurant shop)
        roomGraph
        zone
    |> World.Place.changeOpeningHours Everywhere.Common.restaurantOpeningHours
    |> World.City.addPlace

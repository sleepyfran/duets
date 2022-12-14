module Data.World.Cities.Prague.Restaurants

open Entities
open Data.Items.Food
open Data.Items.Drink

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

    World.Place.create
        ("23b3e34e-25d2-4bd0-ac15-26a7acf45e01" |> Identity.from)
        "Chilli and Lime"
        85<quality>
        (Restaurant shop)
        zone
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

    World.Place.create
        ("5fe0efa6-b0b1-4417-ba0b-a3fb256c5464" |> Identity.from)
        "TAIKO"
        90<quality>
        (Restaurant shop)
        zone
    |> World.City.addPlace

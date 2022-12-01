module Data.Items.Drink

open Entities

/// Defines all the beers available in the game.
module Beer =
    let pilsnerUrquellPint: PurchasableItem =
        { Brand = "Pilsner Urquell"
          Type = Beer(500<milliliter>, 4.4) |> Drink |> Consumable },
        45<dd>

    let kozelPint: PurchasableItem =
        { Brand = "Kozel"
          Type = Beer(500<milliliter>, 4.6) |> Drink |> Consumable },
        45<dd>

    let staropramenPint: PurchasableItem =
        { Brand = "Staropramen"
          Type = Beer(500<milliliter>, 4.7) |> Drink |> Consumable },
        45<dd>

    let gambrinusPint: PurchasableItem =
        { Brand = "Gambrinus"
          Type = Beer(500<milliliter>, 4.3) |> Drink |> Consumable },
        45<dd>

    let cernaHoraPint: PurchasableItem =
        { Brand = "Černa Horá"
          Type = Beer(500<milliliter>, 4.8) |> Drink |> Consumable },
        50<dd>

    let matushkaPint: PurchasableItem =
        { Brand = "Matuška California"
          Type = Beer(500<milliliter>, 5.4) |> Drink |> Consumable },
        55<dd>

    let guinnessPint: PurchasableItem =
        { Brand = "Guinnes"
          Type = Beer(500<milliliter>, 4.3) |> Drink |> Consumable },
        75<dd>

    let estrellaGaliciaBottle: PurchasableItem =
        { Brand = "Estrella Galicia"
          Type = Beer(250<milliliter>, 5.5) |> Drink |> Consumable },
        55<dd>

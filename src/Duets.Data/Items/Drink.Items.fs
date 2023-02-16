module Duets.Data.Items.Drink

open Duets.Entities

module Beer =
    let cernaHoraPint: PurchasableItem =
        { Brand = "Černa Horá"
          Type = Beer(500<milliliter>, 4.8) |> Drink |> Consumable },
        2.2m<dd>

    let estrellaGaliciaBottle: PurchasableItem =
        { Brand = "Estrella Galicia"
          Type = Beer(250<milliliter>, 5.5) |> Drink |> Consumable },
        0.2m<dd>

    let gambrinusPint: PurchasableItem =
        { Brand = "Gambrinus"
          Type = Beer(500<milliliter>, 4.3) |> Drink |> Consumable },
        1.5m<dd>

    let guinnessPint: PurchasableItem =
        { Brand = "Guinnes"
          Type = Beer(500<milliliter>, 4.3) |> Drink |> Consumable },
        2.1m<dd>

    let kozelPint: PurchasableItem =
        { Brand = "Kozel"
          Type = Beer(500<milliliter>, 4.6) |> Drink |> Consumable },
        1.5m<dd>

    let matushkaPint: PurchasableItem =
        { Brand = "Matuška California"
          Type = Beer(500<milliliter>, 5.4) |> Drink |> Consumable },
        2.4m<dd>

    let pilsnerUrquellPint: PurchasableItem =
        { Brand = "Pilsner Urquell"
          Type = Beer(500<milliliter>, 4.4) |> Drink |> Consumable },
        1.8m<dd>

    let saigonBottle: PurchasableItem =
        { Brand = "Saigon"
          Type = Beer(330<milliliter>, 4.9) |> Drink |> Consumable },
        1.8m<dd>

    let sapporoBottle: PurchasableItem =
        { Brand = "Sapporo"
          Type = Beer(330<milliliter>, 4.9) |> Drink |> Consumable },
        1.8m<dd>

    let singhaBottle: PurchasableItem =
        { Brand = "Singha"
          Type = Beer(330<milliliter>, 5.0) |> Drink |> Consumable },
        1.8m<dd>

    let staropramenPint: PurchasableItem =
        { Brand = "Staropramen"
          Type = Beer(500<milliliter>, 4.7) |> Drink |> Consumable },
        1.5m<dd>

module Coffee =
    let espresso: PurchasableItem =
        { Brand = "Espresso"
          Type = Coffee 20<milliliter> |> Drink |> Consumable },
        1.8m<dd>

    let doubleEspresso: PurchasableItem =
        { Brand = "Double espresso"
          Type = Coffee 40<milliliter> |> Drink |> Consumable },
        2.2m<dd>

    let capuccino: PurchasableItem =
        { Brand = "Capuccino"
          Type = Coffee 20<milliliter> |> Drink |> Consumable },
        2.5m<dd>

    let flatWhite: PurchasableItem =
        { Brand = "Flat White"
          Type = Coffee 40<milliliter> |> Drink |> Consumable },
        2.7m<dd>

    let all = [ espresso; doubleEspresso; capuccino; flatWhite ]

module SoftDrinks =
    let cocaColaBottle: PurchasableItem =
        { Brand = "Coca Cola"
          Type = Cola 330<milliliter> |> Drink |> Consumable },
        0.6m<dd>

    let homemadeLemonade: PurchasableItem =
        { Brand = "Homemade lemonade"
          Type = Lemonade 500<milliliter> |> Drink |> Consumable },
        0.9m<dd>

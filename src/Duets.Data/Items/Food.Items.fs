module Duets.Data.Items.Food

open Duets.Entities

module BreakfastFood =
    let avocadoEggSandwich: PurchasableItem =
        { Brand = "Avocado Egg Sandwich"
          Type = Sandwich 150<gram> |> Food |> Consumable },
        3.4m<dd>

    let bltSandwich: PurchasableItem =
        { Brand = "BLT Sandwich"
          Type = Sandwich 200<gram> |> Food |> Consumable },
        3.2m<dd>

    let croissant: PurchasableItem =
        { Brand = "Croissant"
          Type = Croissant 100<gram> |> Food |> Consumable },
        1.2m<dd>

    let fruitPlate: PurchasableItem =
        { Brand = "Fruit Plate"
          Type = Fruits 200<gram> |> Food |> Consumable },
        2.8m<dd>

    let granolaBowl: PurchasableItem =
        { Brand = "Yogurt Granola Bowl"
          Type = GranolaBowl 250<gram> |> Food |> Consumable },
        3m<dd>

module FastFood =
    let genericBurger: PurchasableItem =
        { Brand = "Burger"
          Type = Burger 400<gram> |> Food |> Consumable },
        2.5m<dd>

    let genericChips: PurchasableItem =
        { Brand = "Chips"
          Type = Chips 150<gram> |> Food |> Consumable },
        0.5m<dd>

    let genericFries: PurchasableItem =
        { Brand = "Fries"
          Type = Fries 250<gram> |> Food |> Consumable },
        1.2m<dd>

    let genericNachos: PurchasableItem =
        { Brand = "Nachos"
          Type = Nachos 300<gram> |> Food |> Consumable },
        2.3m<dd>

module JapaneseFood =
    let gyoza: PurchasableItem =
        { Brand = "Gyoza"
          Type = Gyozas 100<gram> |> Food |> Consumable },
        3.3m<dd>

    let misoRamen: PurchasableItem =
        { Brand = "Miso Ramen"
          Type = Ramen 450<gram> |> Food |> Consumable },
        6.4m<dd>

    let tonkotsuRamen: PurchasableItem =
        { Brand = "Tonkotsu Ramen"
          Type = Ramen 450<gram> |> Food |> Consumable },
        6.3m<dd>

    let wakame: PurchasableItem =
        { Brand = "Wakame"
          Type = Wakame 100<gram> |> Food |> Consumable },
        2.5m<dd>

module VietnameseFood =
    let bunBoNamBo: PurchasableItem =
        { Brand = "Bún Bò Nam Bộ"
          Type = BunBo 350<gram> |> Food |> Consumable },
        5.30m<dd>

    let nemCuonBo: PurchasableItem =
        { Brand = "Nem cuốn bò"
          Type = NemCuon 100<gram> |> Food |> Consumable },
        3.50m<dd>

    let nemCuonTom: PurchasableItem =
        { Brand = "Nem cuốn tôm"
          Type = NemCuon 100<gram> |> Food |> Consumable },
        3.35m<dd>

    let phoBo: PurchasableItem =
        { Brand = "Phở Bò"
          Type = PhoBo 350<gram> |> Food |> Consumable },
        5.45m<dd>

let all =
    [ BreakfastFood.croissant
      BreakfastFood.bltSandwich
      BreakfastFood.fruitPlate
      BreakfastFood.granolaBowl
      BreakfastFood.avocadoEggSandwich
      FastFood.genericBurger
      FastFood.genericChips
      FastFood.genericFries
      FastFood.genericNachos
      JapaneseFood.gyoza
      JapaneseFood.wakame
      JapaneseFood.misoRamen
      JapaneseFood.tonkotsuRamen
      VietnameseFood.phoBo
      VietnameseFood.nemCuonBo
      VietnameseFood.nemCuonTom
      VietnameseFood.bunBoNamBo ]

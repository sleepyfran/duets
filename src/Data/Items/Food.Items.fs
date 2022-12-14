module Data.Items.Food

open Entities

module FastFood =
    let genericBurger: PurchasableItem =
        { Brand = "Burger"
          Type = Burger 150<gram> |> Food |> Consumable },
        2.5m<dd>

    let genericChips: PurchasableItem =
        { Brand = "Chips"
          Type = Chips 150<gram> |> Food |> Consumable },
        0.5m<dd>

    let genericFries: PurchasableItem =
        { Brand = "Fries"
          Type = Fries 200<gram> |> Food |> Consumable },
        1.2m<dd>

    let genericNachos: PurchasableItem =
        { Brand = "Nachos"
          Type = Nachos 300<gram> |> Food |> Consumable },
        2.3m<dd>

module JapaneseFood =
    let gyoza: PurchasableItem =
        { Brand = "Gyoza"
          Type = Gyozas 4<times> |> Food |> Consumable },
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
          Type = NemCuon 2<times> |> Food |> Consumable },
        3.50m<dd>

    let nemCuonTom: PurchasableItem =
        { Brand = "Nem cuốn tôm"
          Type = NemCuon 2<times> |> Food |> Consumable },
        3.35m<dd>

    let phoBo: PurchasableItem =
        { Brand = "Phở Bò"
          Type = PhoBo 350<gram> |> Food |> Consumable },
        5.45m<dd>

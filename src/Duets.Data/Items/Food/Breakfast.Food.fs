module Duets.Data.Items.Food.Breakfast

open Duets.Entities

let all: PurchasableItem list =
    [ { Brand = "Avocado Egg Sandwich"
        Type = Healthy 150<gram> |> Food |> Consumable },
      3.4m<dd>

      { Brand = "BLT Sandwich"
        Type = Regular 200<gram> |> Food |> Consumable },
      3.2m<dd>

      { Brand = "Croissant"
        Type = Unhealthy 100<gram> |> Food |> Consumable },
      1.2m<dd>

      { Brand = "Fruit Plate"
        Type = Healthy 200<gram> |> Food |> Consumable },
      2.8m<dd>

      { Brand = "Yogurt Granola Bowl"
        Type = Healthy 250<gram> |> Food |> Consumable },
      3m<dd> ]

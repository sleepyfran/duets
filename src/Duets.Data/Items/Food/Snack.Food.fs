module Duets.Data.Items.Food.Snack

open Duets.Entities

let all: PurchasableItem list =
    [ { Brand = "Potato Chips"
        Type = Unhealthy 150<gram> |> Food |> Consumable },
      2.49m<dd>
      { Brand = "Pretzels"
        Type = Unhealthy 100<gram> |> Food |> Consumable },
      1.99m<dd>
      { Brand = "Chocolate Bar"
        Type = Unhealthy 50<gram> |> Food |> Consumable },
      1.25m<dd>
      { Brand = "Candy Corn"
        Type = Unhealthy 100<gram> |> Food |> Consumable },
      0.99m<dd>
      { Brand = "Popcorn"
        Type = Unhealthy 75<gram> |> Food |> Consumable },
      1.99m<dd>
      { Brand = "Cheese Puffs"
        Type = Unhealthy 100<gram> |> Food |> Consumable },
      1.49m<dd>
      { Brand = "Nachos"
        Type = Unhealthy 200<gram> |> Food |> Consumable },
      2.75m<dd>
      { Brand = "Cookies"
        Type = Unhealthy 50<gram> |> Food |> Consumable },
      1.99m<dd>
      { Brand = "Gummy Bears"
        Type = Unhealthy 100<gram> |> Food |> Consumable },
      1.49m<dd> ]

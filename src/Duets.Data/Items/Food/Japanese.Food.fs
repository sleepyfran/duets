module Duets.Data.Items.Food.Japanese

open Duets.Entities

let all: PurchasableItem list =
    [ { Brand = "Gyoza"
        Type = Regular 100<gram> |> Food |> Consumable },
      3.3m<dd>

      { Brand = "Miso Ramen"
        Type = Healthy 450<gram> |> Food |> Consumable },
      6.4m<dd>

      { Brand = "Tonkotsu Ramen"
        Type = Healthy 450<gram> |> Food |> Consumable },
      6.3m<dd>

      { Brand = "Salmon Nigiri"
        Type = Healthy 100<gram> |> Food |> Consumable },
      7m<dd>

      { Brand = "Tuna Nigiri"
        Type = Healthy 100<gram> |> Food |> Consumable },
      7m<dd>

      { Brand = "Avocado Nigiri"
        Type = Healthy 100<gram> |> Food |> Consumable },
      7m<dd>

      { Brand = "Salmon Maki"
        Type = Healthy 100<gram> |> Food |> Consumable },
      7.2m<dd>

      { Brand = "Avocado Maki"
        Type = Healthy 100<gram> |> Food |> Consumable },
      7.2m<dd>

      { Brand = "California Roll"
        Type = Healthy 150<gram> |> Food |> Consumable },
      7.8m<dd>

      { Brand = "Wakame"
        Type = Healthy 100<gram> |> Food |> Consumable },
      2.5m<dd> ]

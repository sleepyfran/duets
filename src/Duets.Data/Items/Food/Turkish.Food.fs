module Duets.Data.Items.Food.Turkish

open Duets.Entities

let all: PurchasableItem list =
    [ { Brand = "Kebab"
        Type = Unhealthy 500<gram> |> Food |> Consumable },
      5.5m<dd>
      { Brand = "Durum"
        Type = Unhealthy 600<gram> |> Food |> Consumable },
      6.5m<dd>
      { Brand = "Baklava"
        Type = Unhealthy 100<gram> |> Food |> Consumable },
      3.0m<dd>
      { Brand = "Manti"
        Type = Unhealthy 400<gram> |> Food |> Consumable },
      6.0m<dd> ]
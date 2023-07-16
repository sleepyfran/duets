module Duets.Data.Items.Food.USA

open Duets.Entities

let all: PurchasableItem list =
    [ { Brand = "Grilled Pork Ribs"
        Type = Unhealthy 500<gram> |> Food |> Consumable },
      15.0m<dd>

      { Brand = "BBQ Chicken Wings"
        Type = Unhealthy 300<gram> |> Food |> Consumable },
      9.5m<dd>

      { Brand = "Corn on the Cob"
        Type = Healthy 200<gram> |> Food |> Consumable },
      4.0m<dd>

      { Brand = "Steak Sandwich"
        Type = Regular 250<gram> |> Food |> Consumable },
      10.0m<dd>

      { Brand = "Pulled Pork Bun"
        Type = Unhealthy 350<gram> |> Food |> Consumable },
      11.0m<dd>

      { Brand = "Grilled Vegetables"
        Type = Healthy 300<gram> |> Food |> Consumable },
      8.0m<dd>

      { Brand = "Beef Brisket"
        Type = Unhealthy 400<gram> |> Food |> Consumable },
      16.0m<dd>

      { Brand = "BBQ Baked Beans"
        Type = Regular 250<gram> |> Food |> Consumable },
      6.0m<dd>

      { Brand = "Smoked Sausage"
        Type = Regular 200<gram> |> Food |> Consumable },
      9.5m<dd>

      { Brand = "Apple Pie"
        Type = Unhealthy 200<gram> |> Food |> Consumable },
      6.0m<dd> ]

module Duets.Data.Items.Food.Italian

open Duets.Entities

let all: PurchasableItem list =
    [ { Brand = "Margherita Pizza"
        Type = Regular 300<gram> |> Food |> Consumable },
      8.5m<dd>

      { Brand = "Spaghetti Carbonara"
        Type = Unhealthy 250<gram> |> Food |> Consumable },
      9.3m<dd>

      { Brand = "Bruschetta"
        Type = Healthy 150<gram> |> Food |> Consumable },
      5.0m<dd>

      { Brand = "Lasagna"
        Type = Unhealthy 350<gram> |> Food |> Consumable },
      7.8m<dd>

      { Brand = "Tiramisu"
        Type = Unhealthy 150<gram> |> Food |> Consumable },
      3.5m<dd>

      { Brand = "Pesto Pasta"
        Type = Regular 300<gram> |> Food |> Consumable },
      8.7m<dd>

      { Brand = "Caprese Salad"
        Type = Healthy 200<gram> |> Food |> Consumable },
      7.2m<dd>

      { Brand = "Osso Buco"
        Type = Regular 500<gram> |> Food |> Consumable },
      4.3m<dd>

      { Brand = "Risotto alla Milanese"
        Type = Regular 350<gram> |> Food |> Consumable },
      4.6m<dd>

      { Brand = "Gelato"
        Type = Regular 200<gram> |> Food |> Consumable },
      1.5m<dd> ]

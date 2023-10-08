module Duets.Data.Items.Food.Italian

open Duets.Entities

let all: PurchasableItem list =
    [ { Brand = "Pizza Margherita"
        Type = Regular 600<gram> |> Food |> Consumable },
      8.5m<dd>

      { Brand = "Spaghetti Carbonara"
        Type = Regular 450<gram> |> Food |> Consumable },
      9.3m<dd>

      { Brand = "Ravioli al Tartufo"
        Type = Regular 450<gram> |> Food |> Consumable },
      11.3m<dd>

      { Brand = "Fettuccine Alfredo"
        Type = Regular 450<gram> |> Food |> Consumable },
      9.3m<dd>

      { Brand = "Penne alla Vodka"
        Type = Regular 450<gram> |> Food |> Consumable },
      9.5m<dd>

      { Brand = "Penne all'Arrabbiata"
        Type = Regular 450<gram> |> Food |> Consumable },
      9.0m<dd>

      { Brand = "Spaghetti Bolognese"
        Type = Regular 450<gram> |> Food |> Consumable },
      9.2m<dd>

      { Brand = "Penne alla Puttanesca"
        Type = Regular 450<gram> |> Food |> Consumable },
      9.1m<dd>

      { Brand = "Gnocchi Quattro Formaggi"
        Type = Regular 450<gram> |> Food |> Consumable },
      9.0m<dd>

      { Brand = "Bruschetta"
        Type = Regular 150<gram> |> Food |> Consumable },
      5.0m<dd>

      { Brand = "Lasagna Bolognese"
        Type = Unhealthy 350<gram> |> Food |> Consumable },
      7.8m<dd>

      { Brand = "Lasagna Vegetariana"
        Type = Unhealthy 350<gram> |> Food |> Consumable },
      7.2m<dd>

      { Brand = "Pesto Pasta"
        Type = Regular 300<gram> |> Food |> Consumable },
      8.7m<dd>

      { Brand = "Caprese Salad"
        Type = Healthy 200<gram> |> Food |> Consumable },
      7.2m<dd>

      { Brand = "Risotto alla Milanese"
        Type = Regular 350<gram> |> Food |> Consumable },
      4.6m<dd>

      { Brand = "Gelato"
        Type = Unhealthy 200<gram> |> Food |> Consumable },
      1.5m<dd>

      { Brand = "Tiramisu"
        Type = Unhealthy 150<gram> |> Food |> Consumable },
      3.5m<dd> ]

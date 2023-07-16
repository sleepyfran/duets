module Duets.Data.Items.Food.Mexican

open Duets.Entities

let all: PurchasableItem list =
    [ { Brand = "Tacos al Pastor"
        Type = Regular 300<gram> |> Food |> Consumable },
      8.0m<dd>

      { Brand = "Enchiladas"
        Type = Unhealthy 300<gram> |> Food |> Consumable },
      8.5m<dd>

      { Brand = "Guacamole"
        Type = Healthy 150<gram> |> Food |> Consumable },
      4.5m<dd>

      { Brand = "Chiles Rellenos"
        Type = Regular 250<gram> |> Food |> Consumable },
      9.0m<dd>

      { Brand = "Churros"
        Type = Unhealthy 200<gram> |> Food |> Consumable },
      5.0m<dd>

      { Brand = "Quesadilla"
        Type = Regular 250<gram> |> Food |> Consumable },
      7.5m<dd>

      { Brand = "Carnitas"
        Type = Unhealthy 350<gram> |> Food |> Consumable },
      10.0m<dd>

      { Brand = "Pozole"
        Type = Regular 500<gram> |> Food |> Consumable },
      9.8m<dd>

      { Brand = "Sopa Azteca"
        Type = Regular 350<gram> |> Food |> Consumable },
      7.6m<dd>

      { Brand = "Flan"
        Type = Unhealthy 150<gram> |> Food |> Consumable },
      4.2m<dd> ]

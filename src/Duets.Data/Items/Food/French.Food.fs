module Duets.Data.Items.Food.French

open Duets.Entities

let all: PurchasableItem list =
    [ { Brand = "Duck Confit"
        Type = Unhealthy 300<gram> |> Food |> Consumable },
      14.0m<dd>

      { Brand = "Bouillabaisse"
        Type = Regular 500<gram> |> Food |> Consumable },
      13.5m<dd>

      { Brand = "Salade Niçoise"
        Type = Healthy 250<gram> |> Food |> Consumable },
      8.0m<dd>

      { Brand = "Beef Bourguignon"
        Type = Unhealthy 350<gram> |> Food |> Consumable },
      15.8m<dd>

      { Brand = "Crème Brûlée"
        Type = Unhealthy 200<gram> |> Food |> Consumable },
      7.0m<dd>

      { Brand = "Ratatouille"
        Type = Healthy 300<gram> |> Food |> Consumable },
      9.5m<dd>

      { Brand = "Coq au Vin"
        Type = Regular 350<gram> |> Food |> Consumable },
      13.2m<dd>

      { Brand = "Escargot"
        Type = Regular 150<gram> |> Food |> Consumable },
      11.3m<dd>

      { Brand = "Moules Marinières"
        Type = Regular 500<gram> |> Food |> Consumable },
      12.6m<dd>

      { Brand = "Tarte Tatin"
        Type = Unhealthy 200<gram> |> Food |> Consumable },
      6.5m<dd> ]

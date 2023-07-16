module Duets.Data.Items.Food.Czech

open Duets.Entities

let all: PurchasableItem list =
    [ { Brand = "Roasted Duck"
        Type = Unhealthy 300<gram> |> Food |> Consumable },
      5.5m<dd>

      { Brand = "Cabbage Soup"
        Type = Regular 250<gram> |> Food |> Consumable },
      2.3m<dd>

      { Brand = "Bramboráky"
        Type = Regular 200<gram> |> Food |> Consumable },
      3.2m<dd>

      { Brand = "Svíčková"
        Type = Unhealthy 350<gram> |> Food |> Consumable },
      5.8m<dd>

      { Brand = "Apple Strudel"
        Type = Unhealthy 150<gram> |> Food |> Consumable },
      3.7m<dd>

      { Brand = "Koleno"
        Type = Unhealthy 500<gram> |> Food |> Consumable },
      7.5m<dd>

      { Brand = "Buchtičky s krémem"
        Type = Unhealthy 200<gram> |> Food |> Consumable },
      3.7m<dd>

      { Brand = "Hovězí Guláš"
        Type = Regular 350<gram> |> Food |> Consumable },
      5.3m<dd>

      { Brand = "Palačinky"
        Type = Regular 200<gram> |> Food |> Consumable },
      3.8m<dd> ]

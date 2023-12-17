module Duets.Data.Items.Food.French

open Duets.Entities

let all: PurchasableItem list =
    [ Item.Food.create "Duck Confit" 300<gram> Unhealthy, 14.0m<dd>

      Item.Food.create "Bouillabaisse" 500<gram> Regular, 13.5m<dd>

      Item.Food.create "Salade Niçoise" 250<gram> Healthy, 8.0m<dd>

      Item.Food.create "Beef Bourguignon" 350<gram> Unhealthy, 15.8m<dd>

      Item.Food.create "Crème Brûlée" 200<gram> Unhealthy, 7.0m<dd>

      Item.Food.create "Ratatouille" 300<gram> Healthy, 9.5m<dd>

      Item.Food.create "Coq au Vin" 350<gram> Regular, 13.2m<dd>

      Item.Food.create "Escargot" 150<gram> Regular, 11.3m<dd>

      Item.Food.create "Moules Marinières" 500<gram> Regular, 12.6m<dd>

      Item.Food.create "Tarte Tatin" 200<gram> Unhealthy, 6.5m<dd> ]

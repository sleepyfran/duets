module Duets.Data.Items.Food.Turkish

open Duets.Entities

let all: PurchasableItem list =
    [ Item.Food.create "Kebab" 500<gram> Unhealthy, 5.5m<dd>

      Item.Food.create "Durum" 600<gram> Unhealthy, 6.5m<dd>

      Item.Food.create "Baklava" 100<gram> Unhealthy, 3.0m<dd>

      Item.Food.create "Manti" 400<gram> Unhealthy, 6.0m<dd> ]

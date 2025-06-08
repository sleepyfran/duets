module Duets.Data.Items.Food.Japanese

open Duets.Entities

let all: PurchasableItem list =
    [ Item.Food.create "Gyoza" 100<gram> Regular 50, 3.3m<dd>

      Item.Food.create "Miso Ramen" 450<gram> Healthy 70, 6.4m<dd>

      Item.Food.create "Tonkotsu Ramen" 450<gram> Healthy 80, 6.3m<dd>

      Item.Food.create "Salmon Nigiri" 100<gram> Healthy 55, 7m<dd>

      Item.Food.create "Tuna Nigiri" 100<gram> Healthy 55, 7m<dd>

      Item.Food.create "Avocado Nigiri" 100<gram> Healthy 45, 7m<dd>

      Item.Food.create "Salmon Maki" 100<gram> Healthy 55, 7.2m<dd>

      Item.Food.create "Avocado Maki" 100<gram> Healthy 50, 7.2m<dd>

      Item.Food.create "California Roll" 150<gram> Healthy 60, 7.8m<dd>

      Item.Food.create "Wakame" 100<gram> Healthy 10, 2.5m<dd> ]

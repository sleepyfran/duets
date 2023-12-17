module Duets.Data.Items.Food.Snack

open Duets.Entities

let all: PurchasableItem list =
    [ Item.Food.create "Potato Chips" 150<gram> Unhealthy, 2.49m<dd>

      Item.Food.create "Pretzels" 100<gram> Unhealthy, 1.99m<dd>

      Item.Food.create "Chocolate Bar" 50<gram> Unhealthy, 1.25m<dd>

      Item.Food.create "Candy Corn" 100<gram> Unhealthy, 0.99m<dd>

      Item.Food.create "Popcorn" 75<gram> Unhealthy, 1.99m<dd>

      Item.Food.create "Cheese Puffs" 100<gram> Unhealthy, 1.49m<dd>

      Item.Food.create "Nachos" 200<gram> Unhealthy, 2.75m<dd>

      Item.Food.create "Cookies" 50<gram> Unhealthy, 1.99m<dd>

      Item.Food.create "Gummy Bears" 100<gram> Unhealthy, 1.49m<dd> ]

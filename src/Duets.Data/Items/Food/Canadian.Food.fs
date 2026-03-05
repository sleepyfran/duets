module Duets.Data.Items.Food.Canadian

open Duets.Entities

let all: PurchasableItem list =
    [ Item.Food.create "Poutine" 450<gram> Unhealthy 80, 6.5m<dd>

      Item.Food.create "Butter Tart" 80<gram> Unhealthy 55, 2.8m<dd>

      Item.Food.create "Tourtière" 350<gram> Unhealthy 70, 7.2m<dd>

      Item.Food.create "BeaverTails" 150<gram> Unhealthy 50, 4.5m<dd>

      Item.Food.create "Nanaimo Bar" 100<gram> Unhealthy 45, 3.2m<dd>

      Item.Food.create "Montreal Smoked Meat Sandwich" 300<gram> Regular 75, 8.5m<dd>

      Item.Food.create "Maple Salmon" 250<gram> Healthy 65, 9.0m<dd>

      Item.Food.create "Split Pea Soup" 300<gram> Healthy 40, 4.0m<dd>

      Item.Food.create "Bannock" 150<gram> Regular 35, 3.5m<dd> ]

module Duets.Data.Items.Food.British

open Duets.Entities

let all: PurchasableItem list =
    [ Item.Food.create "Fish and Chips" 450<gram> Unhealthy 85, 14.5m<dd>

      Item.Food.create "Full English Breakfast" 500<gram> Unhealthy 90, 13.0m<dd>

      Item.Food.create "Pie and Mash" 400<gram> Unhealthy 75, 9.5m<dd>

      Item.Food.create "Sunday Roast" 550<gram> Regular 95, 19.0m<dd>

      Item.Food.create "Shepherd's Pie" 400<gram> Regular 75, 13.5m<dd>

      Item.Food.create "Bangers and Mash" 400<gram> Unhealthy 70, 12.0m<dd>

      Item.Food.create "Ploughman's Lunch" 350<gram> Regular 55, 11.0m<dd>

      Item.Food.create "Scotch Egg" 150<gram> Unhealthy 35, 5.5m<dd>

      Item.Food.create "Pea and Ham Soup" 300<gram> Healthy 40, 6.5m<dd>

      Item.Food.create "Sticky Toffee Pudding" 180<gram> Unhealthy 50, 7.5m<dd> ]

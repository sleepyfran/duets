module Duets.Data.Items.Food.Breakfast

open Duets.Entities

let all: PurchasableItem list =
    [ Item.Food.create "Avocado Egg Sandwich" 150<gram> Healthy, 3.4m<dd>

      Item.Food.create "Bagel with Cream Cheese" 150<gram> Unhealthy, 2.4m<dd>

      Item.Food.create "BLT Sandwich" 350<gram> Regular, 3.2m<dd>

      Item.Food.create "Croissant" 250<gram> Unhealthy, 1.2m<dd>

      Item.Food.create "Fruit Plate" 200<gram> Healthy, 2.8m<dd>

      Item.Food.create "Yogurt Granola Bowl" 250<gram> Healthy, 3.0m<dd> ]

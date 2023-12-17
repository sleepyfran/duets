module Duets.Data.Items.Food.USA

open Duets.Entities

let all: PurchasableItem list =
    [ Item.Food.create "Grilled Pork Ribs" 500<gram> Unhealthy, 15.0m<dd>

      Item.Food.create "BBQ Chicken Wings" 300<gram> Unhealthy, 9.5m<dd>

      Item.Food.create "Corn on the Cob" 200<gram> Healthy, 4.0m<dd>

      Item.Food.create "Steak Sandwich" 250<gram> Regular, 10.0m<dd>

      Item.Food.create "Pulled Pork Bun" 350<gram> Unhealthy, 11.0m<dd>

      Item.Food.create "Grilled Vegetables" 300<gram> Healthy, 8.0m<dd>

      Item.Food.create "Beef Brisket" 400<gram> Unhealthy, 16.0m<dd>

      Item.Food.create "BBQ Baked Beans" 250<gram> Regular, 6.0m<dd>

      Item.Food.create "Smoked Sausage" 200<gram> Regular, 9.5m<dd>

      Item.Food.create "Apple Pie" 200<gram> Unhealthy, 6.0m<dd> ]

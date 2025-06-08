module Duets.Data.Items.Food.Mexican

open Duets.Entities

let all: PurchasableItem list =
    [ Item.Food.create "Tacos al Pastor" 300<gram> Regular 55, 8.0m<dd>

      Item.Food.create "Enchiladas" 300<gram> Unhealthy 65, 8.5m<dd>

      Item.Food.create "Guacamole" 150<gram> Healthy 15, 4.5m<dd>

      Item.Food.create "Chiles Rellenos" 250<gram> Regular 75, 9.0m<dd>

      Item.Food.create "Churros" 200<gram> Unhealthy 50, 5.0m<dd>

      Item.Food.create "Quesadilla" 250<gram> Regular 20, 7.5m<dd>

      Item.Food.create "Carnitas" 350<gram> Unhealthy 70, 10.0m<dd>

      Item.Food.create "Pozole" 500<gram> Regular 70, 9.8m<dd>

      Item.Food.create "Sopa Azteca" 350<gram> Regular 40, 7.6m<dd>

      Item.Food.create "Flan" 150<gram> Unhealthy 60, 4.2m<dd> ]

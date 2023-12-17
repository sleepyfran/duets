module Duets.Data.Items.Food.Mexican

open Duets.Entities

let all: PurchasableItem list =
    [ Item.Food.create "Tacos al Pastor" 300<gram> Regular, 8.0m<dd>

      Item.Food.create "Enchiladas" 300<gram> Unhealthy, 8.5m<dd>

      Item.Food.create "Guacamole" 150<gram> Healthy, 4.5m<dd>

      Item.Food.create "Chiles Rellenos" 250<gram> Regular, 9.0m<dd>

      Item.Food.create "Churros" 200<gram> Unhealthy, 5.0m<dd>

      Item.Food.create "Quesadilla" 250<gram> Regular, 7.5m<dd>

      Item.Food.create "Carnitas" 350<gram> Unhealthy, 10.0m<dd>

      Item.Food.create "Pozole" 500<gram> Regular, 9.8m<dd>

      Item.Food.create "Sopa Azteca" 350<gram> Regular, 7.6m<dd>

      Item.Food.create "Flan" 150<gram> Unhealthy, 4.2m<dd> ]

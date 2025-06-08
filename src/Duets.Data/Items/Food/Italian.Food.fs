module Duets.Data.Items.Food.Italian

open Duets.Entities

let all: PurchasableItem list =
    [ Item.Food.create "Pizza Margherita" 600<gram> Regular 60, 8.5m<dd>

      Item.Food.create "Spaghetti Carbonara" 450<gram> Regular 55, 9.3m<dd>

      Item.Food.create "Ravioli al Tartufo" 450<gram> Regular 65, 11.3m<dd>

      Item.Food.create "Fettuccine Alfredo" 450<gram> Regular 40, 9.3m<dd>

      Item.Food.create "Penne alla Vodka" 450<gram> Regular 45, 9.5m<dd>

      Item.Food.create "Penne all'Arrabbiata" 450<gram> Regular 35, 9.0m<dd>

      Item.Food.create "Spaghetti Bolognese" 450<gram> Regular 50, 9.2m<dd>

      Item.Food.create "Penne alla Puttanesca" 450<gram> Regular 50, 9.1m<dd>

      Item.Food.create "Gnocchi Quattro Formaggi" 450<gram> Regular 55, 9.0m<dd>

      Item.Food.create "Bruschetta" 150<gram> Regular 15, 5.0m<dd>

      Item.Food.create "Lasagna Bolognese" 350<gram> Unhealthy 70, 7.8m<dd>

      Item.Food.create "Lasagna Vegetariana" 350<gram> Unhealthy 60, 7.2m<dd>

      Item.Food.create "Pesto Pasta" 300<gram> Regular 30, 8.7m<dd>

      Item.Food.create "Caprese Salad" 200<gram> Healthy 5, 7.2m<dd>

      Item.Food.create "Risotto alla Milanese" 350<gram> Regular 70, 4.6m<dd>

      Item.Food.create "Gelato" 200<gram> Unhealthy 0, 1.5m<dd>

      Item.Food.create "Tiramisu" 150<gram> Unhealthy 55, 3.5m<dd> ]

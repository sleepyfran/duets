module Duets.Data.Items.Food.Korean

open Duets.Entities

let all: PurchasableItem list =
    [ Item.Food.create "Bibimbap" 450<gram> Healthy 75, 7.5m<dd>

      Item.Food.create "Kimchi Jjigae" 400<gram> Healthy 60, 6.5m<dd>

      Item.Food.create "Bulgogi" 300<gram> Healthy 80, 9.0m<dd>

      Item.Food.create "Tteokbokki" 250<gram> Regular 55, 5.0m<dd>

      Item.Food.create "Japchae" 300<gram> Healthy 65, 8.0m<dd>

      Item.Food.create "Kimchi Fried Rice" 400<gram> Regular 60, 6.0m<dd>

      Item.Food.create "Sundubu Jjigae" 400<gram> Healthy 55, 6.8m<dd>

      Item.Food.create "Galbi" 350<gram> Healthy 85, 12.0m<dd>

      Item.Food.create "Kimbap" 200<gram> Healthy 50, 4.5m<dd>

      Item.Food.create "Haemul Pajeon" 300<gram> Regular 65, 7.0m<dd> ]

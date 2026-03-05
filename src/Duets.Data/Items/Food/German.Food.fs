module Duets.Data.Items.Food.German

open Duets.Entities

let all: PurchasableItem list =
    [ Item.Food.create "Currywurst" 300<gram> Unhealthy 50, 5.0m<dd>

      Item.Food.create "Wiener Schnitzel" 350<gram> Unhealthy 85, 14.0m<dd>

      Item.Food.create "Bratwurst" 250<gram> Unhealthy 50, 5.5m<dd>

      Item.Food.create "Kartoffelpuffer" 200<gram> Regular 40, 7.0m<dd>

      Item.Food.create "Käsespätzle" 300<gram> Regular 60, 9.5m<dd>

      Item.Food.create "Sauerbraten" 400<gram> Regular 75, 15.0m<dd>

      Item.Food.create "Döner Kebab" 350<gram> Regular 50, 5.0m<dd>

      Item.Food.create "Königsberger Klopse" 300<gram> Regular 60, 12.0m<dd>

      Item.Food.create "Brezel" 100<gram> Regular 20, 2.0m<dd>

      Item.Food.create "Apfelstrudel" 200<gram> Unhealthy 60, 6.5m<dd> ]

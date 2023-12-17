module Duets.Data.Items.Food.Czech

open Duets.Entities

let all: PurchasableItem list =
    [ Item.Food.create "Roasted Duck" 300<gram> Unhealthy, 5.5m<dd>

      Item.Food.create "Cabbage Soup" 250<gram> Regular, 2.3m<dd>

      Item.Food.create "Bramboráky" 200<gram> Regular, 3.2m<dd>

      Item.Food.create "Svíčková" 550<gram> Unhealthy, 5.8m<dd>

      Item.Food.create "Smažený sýr" 350<gram> Unhealthy, 3.2m<dd>

      Item.Food.create "Koleno" 500<gram> Unhealthy, 7.5m<dd>

      Item.Food.create "Buchtičky s krémem" 200<gram> Unhealthy, 3.7m<dd>

      Item.Food.create "Hovězí Guláš" 550<gram> Regular, 5.3m<dd>

      Item.Food.create "Palacinky" 200<gram> Regular, 3.8m<dd> ]

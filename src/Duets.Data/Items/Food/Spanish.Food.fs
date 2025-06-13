module Duets.Data.Items.Food.Spanish

open Duets.Entities

let all: PurchasableItem list =
    [ Item.Food.create "Paella" 400<gram> Regular 80, 13.0m<dd>
      Item.Food.create "Tortilla de Patatas" 250<gram> Healthy 40, 7.0m<dd>
      Item.Food.create "Gazpacho" 300<gram> Healthy 30, 6.5m<dd>
      Item.Food.create "Pulpo a la Gallega" 250<gram> Regular 75, 12.0m<dd>
      Item.Food.create "Patatas Bravas" 200<gram> Unhealthy 30, 5.0m<dd>
      Item.Food.create "Croquetas de Jamón" 180<gram> Unhealthy 50, 6.2m<dd>
      Item.Food.create "Churros con Chocolate" 180<gram> Unhealthy 35, 5.5m<dd>
      Item.Food.create "Fabada Asturiana" 350<gram> Regular 70, 10.5m<dd>
      Item.Food.create "Calamares a la Romana" 220<gram> Unhealthy 60, 8.0m<dd>
      Item.Food.create "Pimientos de Padrón" 120<gram> Healthy 20, 4.5m<dd>
      Item.Food.create "Empanada Gallega" 200<gram> Regular 45, 6.8m<dd>
      Item.Food.create "Cochinillo Asado" 400<gram> Unhealthy 90, 16.0m<dd>
      Item.Food.create "Salmorejo" 300<gram> Healthy 35, 6.7m<dd>
      Item.Food.create "Gambas al Ajillo" 180<gram> Regular 55, 9.0m<dd>
      Item.Food.create "Callos a la Madrileña" 320<gram> Regular 80, 11.5m<dd>
      Item.Food.create "Ensaimada" 140<gram> Unhealthy 35, 5.8m<dd> ]

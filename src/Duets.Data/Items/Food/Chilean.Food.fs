module Duets.Data.Items.Food.Chilean

open Duets.Entities

let all: PurchasableItem list =
    [ Item.Food.create "Empanada de Pino" 200<gram> Regular 55, 4.5m<dd>

      Item.Food.create "Pastel de Choclo" 500<gram> Healthy 80, 8.0m<dd>

      Item.Food.create "Cazuela de Vacuno" 500<gram> Healthy 70, 7.5m<dd>

      Item.Food.create "Completo Italiano" 250<gram> Unhealthy 60, 4.0m<dd>

      Item.Food.create "Chacarero" 300<gram> Regular 65, 5.0m<dd>

      Item.Food.create "Porotos Granados" 400<gram> Healthy 65, 6.5m<dd>

      Item.Food.create "Humitas" 250<gram> Healthy 50, 4.5m<dd>

      Item.Food.create "Sopaipilla Pasada" 150<gram> Unhealthy 40, 2.5m<dd>

      Item.Food.create "Lomo a lo Pobre" 450<gram> Unhealthy 85, 9.0m<dd>

      Item.Food.create "Curanto en Olla" 600<gram> Healthy 90, 12.0m<dd> ]

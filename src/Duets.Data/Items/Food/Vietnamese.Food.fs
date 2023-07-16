module Duets.Data.Items.Food.Vietnamese

open Duets.Entities

let all: PurchasableItem list =
    [ { Brand = "Bún Bò Nam Bộ"
        Type = Healthy 350<gram> |> Food |> Consumable },
      5.30m<dd>

      { Brand = "Nem cuốn bò"
        Type = Healthy 100<gram> |> Food |> Consumable },
      3.50m<dd>

      { Brand = "Nem cuốn tôm"
        Type = Healthy 100<gram> |> Food |> Consumable },
      3.35m<dd>

      { Brand = "Phở Bò"
        Type = Healthy 350<gram> |> Food |> Consumable },
      5.45m<dd> ]

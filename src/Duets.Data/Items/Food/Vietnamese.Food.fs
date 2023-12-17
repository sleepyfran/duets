module Duets.Data.Items.Food.Vietnamese

open Duets.Entities

let all: PurchasableItem list =
    [ Item.Food.create "Bún Bò Nam Bộ" 350<gram> Healthy, 5.30m<dd>

      Item.Food.create "Nem cuốn bò" 100<gram> Healthy, 3.50m<dd>

      Item.Food.create "Nem cuốn tôm" 100<gram> Healthy, 3.35m<dd>

      Item.Food.create "Phở Bò" 350<gram> Healthy, 5.45m<dd> ]

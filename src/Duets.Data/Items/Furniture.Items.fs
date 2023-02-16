module Duets.Data.Items.Furniture

open Duets.Entities

module Bed =
    let ikeaBed: PurchasableItem =
        { Brand = "IKEA"
          Type = Bed |> Furniture |> Interactive },
        450m<dd>

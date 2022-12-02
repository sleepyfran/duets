module Data.Items.Furniture

open Entities

module Bed =
    let ikeaBed: PurchasableItem =
        { Brand = "IKEA"
          Type = Bed |> Furniture |> Interactive },
        450m<dd>

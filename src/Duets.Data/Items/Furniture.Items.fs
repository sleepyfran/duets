module Duets.Data.Items.Furniture

open Duets.Entities

module Bed =
    let ikeaBed: PurchasableItem =
        { Brand = "IKEA"
          Type = Bed |> Furniture |> Interactive },
        450m<dd>

module BilliardTable =
    let sonomaTable: PurchasableItem =
        { Brand = "Sonoma"
          Type = BilliardTable |> Furniture |> Interactive },
        3400m<dd>

module Stove =
    let lgStove: PurchasableItem =
        { Brand = "LG"
          Type = Stove |> Furniture |> Interactive },
        650m<dd>

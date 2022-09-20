module Data.Items.Electronics

open Entities

module GameConsole =
    let xbox: PurchasableItem =
        { Brand = "Xbox"
          Type = GameConsole |> Electronics |> Interactive },
        7000<dd>

module Tv =
    let lgTv: PurchasableItem =
        { Brand = "LG"
          Type = TV |> Electronics |> Interactive },
        8000<dd>

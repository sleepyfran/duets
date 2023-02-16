module Duets.Data.Items.Electronics

open Duets.Entities

module GameConsole =
    let xbox: PurchasableItem =
        { Brand = "Xbox Series X"
          Type = GameConsole |> Electronics |> Interactive },
        550m<dd>

module Tv =
    let lgTv: PurchasableItem =
        { Brand = "LG"
          Type = TV |> Electronics |> Interactive },
        850m<dd>

module Duets.Data.Items.Electronics

open Duets.Entities

module Dartboard =
    let dartboard: PurchasableItem =
        { Brand = "Bull's"
          Type = Dartboard |> Electronics |> Interactive },
        230m<dd>

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

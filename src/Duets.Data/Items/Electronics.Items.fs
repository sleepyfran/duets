module Duets.Data.Items.Electronics

open Duets.Entities

module Dartboard =
    let dartboard: PurchasableItem =
        { Brand = "Bull's Dartboard"
          Properties = [ Playable(Darts) ] },
        230m<dd>

module GameConsole =
    let xbox: PurchasableItem =
        { Brand = "Xbox Series X"
          Properties = [ Playable(VideoGame) ] },
        550m<dd>

module Tv =
    let lgTv: PurchasableItem =
        { Brand = "LG OLED TV"
          Properties = [ Watchable ] },
        850m<dd>

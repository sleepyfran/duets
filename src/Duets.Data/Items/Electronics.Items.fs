module Duets.Data.Items.Electronics

open Duets.Entities

module Dartboard =
    let dartboard: PurchasableItem =
        { Brand = "Bull's"
          Name = "Dartboard"
          Properties = [ Playable(Darts) ] },
        230m<dd>

module GameConsole =
    let xbox: PurchasableItem =
        { Brand = "Microsoft"
          Name = "Xbox Series X"
          Properties = [ Playable(VideoGame) ] },
        550m<dd>

module Tv =
    let lgTv: PurchasableItem =
        { Brand = "LG"
          Name = "TV"
          Properties = [ Watchable ] },
        850m<dd>

module Duets.Data.Items.Furniture

open Duets.Entities

module Bed =
    let ikeaBed: PurchasableItem =
        { Brand = "IKEA bed"
          Properties = [ Sleepable ] },
        450m<dd>

module BilliardTable =
    let sonomaTable: PurchasableItem =
        { Brand = "Sonoma billiard table"
          Properties = [ Playable(Billiard) ] },
        3400m<dd>

module Stove =
    let lgStove: PurchasableItem =
        { Brand = "LG stove"
          Properties = [ Cookware ] },
        650m<dd>

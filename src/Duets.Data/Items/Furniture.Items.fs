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

module Storage =
    let samsungFridge: PurchasableItem =
        { Brand = "Samsung fridge"
          Properties = [ Storage(Fridge, []) ] },
        750m<dd>

    let ikeaShelf: PurchasableItem =
        { Brand = "IKEA shelf"
          Properties = [ Storage(Shelf, []) ] },
        150m<dd>

module Stove =
    let lgStove: PurchasableItem =
        { Brand = "LG stove"
          Properties = [ Cookware ] },
        650m<dd>

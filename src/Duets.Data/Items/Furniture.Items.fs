module Duets.Data.Items.Furniture

open Duets.Entities

module Bed =
    let ikeaBed: PurchasableItem =
        { Brand = "IKEA"
          Name = "Bed"
          Properties = [ Sleepable ] },
        450m<dd>

module BilliardTable =
    let sonomaTable: PurchasableItem =
        { Brand = "Sonoma"
          Name = "Billiard table"
          Properties = [ Playable(Billiard) ] },
        3400m<dd>

module Storage =
    let samsungFridge: PurchasableItem =
        { Brand = "Samsung"
          Name = "Fridge"
          Properties = [ Storage(Fridge, []) ] },
        750m<dd>

    let ikeaShelf: PurchasableItem =
        { Brand = "IKEA"
          Name = "Shelf"
          Properties = [ Storage(Shelf, []) ] },
        150m<dd>

module Stove =
    let lgStove: PurchasableItem =
        { Brand = "LG"
          Name = "Stove"
          Properties = [ Cookware ] },
        650m<dd>

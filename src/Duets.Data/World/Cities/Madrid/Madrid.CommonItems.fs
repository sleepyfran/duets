module Duets.Data.World.Cities.Madrid.CityCommonItems

open Duets.Data.Items

/// List of most common beers sold in Madrid pubs.
let beers =
    [ Drink.Beer.pilsnerUrquellPint
      Drink.Beer.estrellaGaliciaBottle
      Drink.Beer.guinnessPint ]

/// List of most common food sold in Madrid pubs.
let pubFood = [ Food.FastFood.genericNachos; Food.FastFood.genericChips ]

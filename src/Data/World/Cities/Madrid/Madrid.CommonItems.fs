module Data.World.Cities.Madrid.CommonItems

open Data.Items

/// List of most common beers sold in Madrid pubs.
let pubDrinks =
    [ Drink.Beer.pilsnerUrquellPint
      Drink.Beer.estrellaGaliciaBottle
      Drink.Beer.guinnessPint ]

/// List of most common food sold in Madrid pubs.
let pubFood = [ Food.FastFood.genericNachos; Food.FastFood.genericChips ]

module Data.World.Cities.Madrid.Shops

open Data.Items

/// List of most common beers sold in Madrid pubs.
let commonPubDrinks =
    [ Drink.Beer.pilsnerUrquellPint
      Drink.Beer.estrellaGaliciaBottle
      Drink.Beer.guinnessPint ]

/// List of most common food sold in Madrid pubs.
let commonPubFood =
    [ Food.FastFood.genericNachos
      Food.FastFood.genericChips ]

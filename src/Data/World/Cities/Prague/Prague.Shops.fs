module Data.World.Cities.Prague.Shops

open Data.Items

/// List of most common beers sold in pubs Prague.
let commonPubDrinks =
    [ Drink.Beer.pilsnerUrquellPint
      Drink.Beer.kozelPint
      Drink.Beer.staropramenPint
      Drink.Beer.gambrinusPint
      Drink.Beer.cernaHoraPint
      Drink.Beer.matushkaPint ]

/// List of most common food sold in pubs in Prague.
let commonPubFood =
    [ Food.FastFood.genericBurger
      Food.FastFood.genericFries
      Food.FastFood.genericNachos ]

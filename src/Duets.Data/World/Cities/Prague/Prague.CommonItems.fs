module Duets.Data.World.Cities.Prague.CommonItems

open Duets.Data.Items

/// List of most common beers sold in pubs Prague.
let pubDrinks =
    [ Drink.Beer.pilsnerUrquellPint
      Drink.Beer.kozelPint
      Drink.Beer.staropramenPint
      Drink.Beer.gambrinusPint
      Drink.Beer.cernaHoraPint
      Drink.Beer.matushkaPint ]

/// List of most common food sold in pubs in Prague.
let pubFood =
    [ Food.FastFood.genericBurger
      Food.FastFood.genericFries
      Food.FastFood.genericNachos ]

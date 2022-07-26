/// Defines all the shops that are specific to Prague, so that it contains
/// items that can only be found in this city.
module Data.World.Cities.Prague.Shops

open Data.Items
open Entities

/// Creates a bar with all generic items that are usually in a bar in Prague,
/// applying the given modifier to the prices.
let genericBar modifier =
    { AvailableItems =
        [ Drink.Beer.pilsnerUrquellPint
          Drink.Beer.kozelPint
          Drink.Beer.staropramenPint
          Drink.Beer.gambrinusPint
          Drink.Beer.cernaHoraPint
          Drink.Beer.matushkaPint ]
      PriceModifier = modifier }

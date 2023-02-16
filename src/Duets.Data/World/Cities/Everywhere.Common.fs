module Duets.Data.World.Everywhere.Common

open Duets.Entities
open Duets.Data.Items.Drink
open Duets.Data.Items.Food

/// Usual bar opening hours around the world.
let barOpeningHours =
    PlaceOpeningHours.OpeningHours(
        Calendar.everyDay,
        [ Midday; Afternoon; Evening; Night; Midnight ]
    )

/// Usual cafe opening hours around the world.
let cafeOpeningHours =
    PlaceOpeningHours.OpeningHours(
        Calendar.everyDay,
        [ EarlyMorning; Morning; Midday; Afternoon; Evening ]
    )

/// Usual concert space hours around the world.
let concertSpaceOpeningHours =
    PlaceOpeningHours.OpeningHours(
        Calendar.everyDay,
        [ Afternoon; Evening; Night ]
    )

/// Usual restaurant hours around the world.
let restaurantOpeningHours =
    PlaceOpeningHours.OpeningHours(
        Calendar.everyDay,
        [ Midday; Afternoon; Evening; Night ]
    )

/// Usual opening hours for service places like studios or rehearsal places.
let servicesOpeningHours =
    PlaceOpeningHours.OpeningHours(
        Calendar.everyDay,
        [ Midday; Afternoon; Evening ]
    )

/// Usual stuff in coffee shops everywhere.
let coffeeShopItems =
    [ BreakfastFood.avocadoEggSandwich
      BreakfastFood.bltSandwich
      BreakfastFood.croissant
      BreakfastFood.fruitPlate
      BreakfastFood.granolaBowl ]
    @ Coffee.all

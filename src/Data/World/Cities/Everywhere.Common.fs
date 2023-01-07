module Data.World.Everywhere.Common

open Entities

/// Usual bar opening hours around the world.
let barOpeningHours =
    PlaceOpeningHours.OpeningHours(
        World.Place.OpeningHours.everyDay,
        [ Midday; Afternoon; Evening; Night; Midnight ]
    )

/// Usual cafe opening hours around the world.
let cafeOpeningHours =
    PlaceOpeningHours.OpeningHours(
        World.Place.OpeningHours.everyDay,
        [ EarlyMorning; Morning; Midday; Afternoon; Evening ]
    )

/// Usual restaurant hours around the world.
let restaurantOpeningHours =
    PlaceOpeningHours.OpeningHours(
        World.Place.OpeningHours.everyDay,
        [ Midday; Afternoon; Evening; Night ]
    )

/// Usual opening hours for service places like studios or rehearsal places.
let servicesOpeningHours =
    PlaceOpeningHours.OpeningHours(
        World.Place.OpeningHours.everyDay,
        [ Midday; Afternoon; Evening ]
    )

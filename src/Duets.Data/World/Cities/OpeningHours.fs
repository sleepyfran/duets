module Duets.Data.World.OpeningHours

open Duets.Entities

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

/// Usual kebab opening hours around the world.
let kebabOpeningHours =
    PlaceOpeningHours.OpeningHours(
        Calendar.everyDay,
        [ EarlyMorning; Midday; Afternoon; Evening; Night; Midnight ]
    )

/// Usual gym hours around the world.
let gymOpeningHours =
    PlaceOpeningHours.OpeningHours(
        Calendar.everyDay,
        [ EarlyMorning; Morning; Midday; Afternoon; Evening; Night ]
    )

/// Usual opening hours for service places like studios or rehearsal places.
let servicesOpeningHours =
    PlaceOpeningHours.OpeningHours(
        Calendar.everyDay,
        [ Morning; Midday; Afternoon; Evening ]
    )

/// Usual opening hours for radio studios.
let radioStudioOpeningHours =
    PlaceOpeningHours.OpeningHours(
        Calendar.everyDay,
        [ EarlyMorning; Morning; Midday; Afternoon; Evening ]
    )

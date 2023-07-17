module Duets.Data.World.Cities.Common

open Duets.Data.World
open Duets.Entities

/// Creates a bar with the given name, quality and zone.
let createBar (name, quality, zone) =
    World.Place.create name quality Bar Everywhere.Common.barRoomLayout zone
    |> World.Place.changeOpeningHours Everywhere.Common.barOpeningHours

/// Creates a cafe with the given name, quality and zone.
let createCafe (name, quality, zone) =
    World.Place.create name quality Cafe Everywhere.Common.cafeRoomLayout zone
    |> World.Place.changeOpeningHours Everywhere.Common.cafeOpeningHours

/// Creates a concert space with the given name, capacity, quality and zone.
let createConcertSpace (name, capacity, zone, quality, layout) =
    World.Place.create
        name
        quality
        (ConcertSpace { Capacity = capacity })
        layout
        zone
    |> World.Place.changeOpeningHours Everywhere.Common.concertSpaceOpeningHours

/// Creates a rehearsal space with the given name, quality, price and zone.
let createRehearsalSpace (name, quality, price, zone) =
    World.Place.create
        name
        quality
        (RehearsalSpace { Price = price })
        Everywhere.Common.rehearsalSpaceLayout
        zone
    |> World.Place.changeOpeningHours Everywhere.Common.servicesOpeningHours

/// Creates a restaurant with the given name, quality, cuisine and zone.
let createRestaurant (name, quality, cuisine, zone) =
    World.Place.create
        name
        quality
        Restaurant
        (Everywhere.Common.restaurantRoomLayout cuisine)
        zone
    |> World.Place.changeOpeningHours Everywhere.Common.restaurantOpeningHours

/// Creates a studio with the given name, quality, price per song and zone.
let createStudio (name, quality, pricePerSong, zone, producer) =
    let studio =
        { Producer = producer
          PricePerSong = pricePerSong }

    World.Place.create
        name
        quality
        (Studio studio)
        Everywhere.Common.studioLayout
        zone
    |> World.Place.changeOpeningHours Everywhere.Common.servicesOpeningHours

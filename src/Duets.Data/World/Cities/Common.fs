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

/// Creates a casino with the given name, quality and zone.
let createCasino (name, quality, zone) =
    World.Place.create name quality Casino Everywhere.Common.casinoLayout zone

/// Creates a concert space with the given name, capacity, quality and zone.
let createConcertSpace (name, capacity, zone, quality, layout) =
    World.Place.create
        name
        quality
        (ConcertSpace { Capacity = capacity })
        layout
        zone
    |> World.Place.changeOpeningHours Everywhere.Common.concertSpaceOpeningHours

/// Creates a gym with the given name, quality and zone.
let createGym (city: City) (name, quality, zone) =
    let place =
        World.Place.create name quality Gym Everywhere.Common.gymLayout zone

    let entranceChip = Item.Chip.createFor city.Id place.Id

    place
    |> World.Place.changeRoom Ids.Gym.changingRoom (function
        | Some room ->
            let requiredItems =
                { ComingFrom = Ids.Gym.lobby
                  Items = [ entranceChip ] }

            room
            |> World.Room.changeRequiredItemForEntrance requiredItems
            |> Some
        | _ -> None)
    |> World.Place.changeOpeningHours Everywhere.Common.gymOpeningHours

/// Creates a hotel with the given name, quality, price per night and zone.
let createHotel (name, quality, pricePerNight, zone) =
    World.Place.create
        name
        quality
        (Hotel { PricePerNight = pricePerNight })
        Everywhere.Common.hotelLayout
        zone

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

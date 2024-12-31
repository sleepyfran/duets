module Duets.Data.World.Cities.PlaceCreators

open Duets.Data.World
open Duets.Entities

/// Creates a bar with the given name, quality and zone.
let createBar (name, quality) =
    World.Place.create name quality Bar Layouts.barRoomLayout
    |> World.Place.changeOpeningHours OpeningHours.barOpeningHours

/// Creates a bookstore with the given name, quality and zone.
let createBookstore (name, quality) =
    World.Place.create name quality Bookstore Layouts.bookstoreLayout
    |> World.Place.changeOpeningHours OpeningHours.servicesOpeningHours

/// Creates a cafe with the given name, quality and zone.
let createCafe (name, quality) =
    World.Place.create name quality Cafe Layouts.cafeRoomLayout
    |> World.Place.changeOpeningHours OpeningHours.cafeOpeningHours

/// Creates a casino with the given name, quality and zone.
let createCasino (name, quality) =
    World.Place.create name quality Casino Layouts.casinoLayout

/// Creates a concert space with the given name, capacity, quality and zone.
let createConcertSpace (name, capacity, quality, layout) =
    World.Place.create
        name
        quality
        (ConcertSpace { Capacity = capacity })
        layout
    |> World.Place.changeOpeningHours OpeningHours.concertSpaceOpeningHours

/// Creates a gym with the given name, quality and zone.
let createGym (city: City) (name, quality) =
    let place = World.Place.create name quality Gym Layouts.gymLayout

    let entranceChip = Item.Chip.createFor city.Id place.Id

    place
    |> World.Place.changeRoom Ids.Gym.changingRoom (function
        | Some room ->
            let requiredItems =
                { ComingFrom = Ids.Common.lobby
                  Items = [ entranceChip ] }

            room
            |> World.Room.changeRequiredItemForEntrance requiredItems
            |> Some
        | _ -> None)
    |> World.Place.changeOpeningHours OpeningHours.gymOpeningHours

/// Creates a hotel with the given name, quality, price per night and zone.
let createHotel (name, quality, pricePerNight) =
    World.Place.create
        name
        quality
        (Hotel { PricePerNight = pricePerNight })
        Layouts.hotelLayout

/// Creates a merchandise workshop with the given name and zone.
let createMerchandiseWorkshop name =
    World.Place.create
        name
        100<quality>
        MerchandiseWorkshop
        Layouts.merchandiseWorkshopLayout

/// Creates a metro station with the given name and lines.
let createMetro name =
    World.Place.create name 100<quality> MetroStation Layouts.metroLayout

/// Creates a rehearsal space with the given name, quality, price and zone.
let createRehearsalSpace (name, quality, price) =
    World.Place.create
        name
        quality
        (RehearsalSpace { Price = price })
        Layouts.rehearsalSpaceLayout
    |> World.Place.changeOpeningHours OpeningHours.servicesOpeningHours

/// Creates a restaurant with the given name, quality, cuisine and zone.
let createRestaurant (name, quality, cuisine) =
    let place =
        World.Place.create
            name
            quality
            Restaurant
            (Layouts.restaurantRoomLayout cuisine)

    let openingHours =
        match cuisine with
        | Turkish -> PlaceOpeningHours.AlwaysOpen
        | _ -> OpeningHours.restaurantOpeningHours

    (openingHours, place) ||> World.Place.changeOpeningHours

/// Creates a studio with the given name, quality, price per song and zone.
let createStudio (name, quality, pricePerSong, producer) =
    let studio =
        { Producer = producer
          PricePerSong = pricePerSong }

    World.Place.create name quality (Studio studio) Layouts.studioLayout
    |> World.Place.changeOpeningHours OpeningHours.servicesOpeningHours

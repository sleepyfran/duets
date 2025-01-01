module Duets.Data.World.Cities.PlaceCreators

open Duets.Data.World
open Duets.Entities

/// Creates a bar with the given name, quality and zone.
let createBar streetId (name, quality, zoneId) =
    World.Place.create name quality Bar Layouts.barRoomLayout zoneId
    |> World.Place.changeOpeningHours OpeningHours.barOpeningHours
    |> World.Place.addExit Ids.Common.bar streetId

/// Creates a bookstore with the given name, quality and zone.
let createBookstore streetId (name, quality, zoneId) =
    World.Place.create name quality Bookstore Layouts.bookstoreLayout zoneId
    |> World.Place.changeOpeningHours OpeningHours.servicesOpeningHours
    |> World.Place.addExit Ids.Bookstore.readingRoom streetId

/// Creates a cafe with the given name, quality and zone.
let createCafe streetId (name, quality, zoneId) =
    World.Place.create name quality Cafe Layouts.cafeRoomLayout zoneId
    |> World.Place.changeOpeningHours OpeningHours.cafeOpeningHours
    |> World.Place.addExit Ids.Common.cafe streetId

/// Creates a casino with the given name, quality and zone.
let createCasino streetId (name, quality, zoneId) =
    World.Place.create name quality Casino Layouts.casinoLayout zoneId
    |> World.Place.addExit Ids.Common.lobby streetId

/// Creates a concert space with the given name, capacity, quality and zone.
let createConcertSpace streetId (name, capacity, quality, layout, zoneId) =
    World.Place.create
        name
        quality
        (ConcertSpace { Capacity = capacity })
        layout
        zoneId
    |> World.Place.changeOpeningHours OpeningHours.concertSpaceOpeningHours
    |> World.Place.addExit Ids.Common.lobby streetId

/// Creates a gym with the given name, quality and zone.
let createGym (city: City) streetId (name, quality, zoneId) =
    let place = World.Place.create name quality Gym Layouts.gymLayout zoneId

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
    |> World.Place.addExit Ids.Common.lobby streetId

/// Creates a home with the given zone.
/// TODO: Allow different types of homes depending on the zone, how much the rent is, etc.
let createHome streetId zoneId =
    World.Place.create "Home" 100<quality> Home Layouts.homeLayout zoneId
    |> World.Place.addExit Ids.Home.livingRoom streetId

/// Creates a hotel with the given name, quality, price per night and zone.
let createHotel streetId (name, quality, pricePerNight, zoneId) =
    World.Place.create
        name
        quality
        (Hotel { PricePerNight = pricePerNight })
        Layouts.hotelLayout
        zoneId
    |> World.Place.addExit Ids.Common.lobby streetId

/// Creates a merchandise workshop with the given name and zone.
let createMerchandiseWorkshop streetId (name, zoneId) =
    World.Place.create
        name
        100<quality>
        MerchandiseWorkshop
        Layouts.merchandiseWorkshopLayout
        zoneId
    |> World.Place.changeOpeningHours OpeningHours.servicesOpeningHours
    |> World.Place.addExit Ids.Workshop.workshop streetId

/// Creates a metro station with the given name and lines.
let createMetro streetId (name, zoneId) =
    World.Place.create name 100<quality> MetroStation Layouts.metroLayout zoneId
    |> World.Place.addExit Ids.Common.lobby streetId

/// Creates a rehearsal space with the given name, quality, price and zone.
let createRehearsalSpace streetId (name, quality, price, zoneId) =
    World.Place.create
        name
        quality
        (RehearsalSpace { Price = price })
        Layouts.rehearsalSpaceLayout
        zoneId
    |> World.Place.changeOpeningHours OpeningHours.servicesOpeningHours
    |> World.Place.addExit Ids.Common.lobby streetId

/// Creates a restaurant with the given name, quality, cuisine and zone.
let createRestaurant streetId (name, quality, cuisine, zoneId) =
    let place =
        World.Place.create
            name
            quality
            Restaurant
            (Layouts.restaurantRoomLayout cuisine)
            zoneId

    let openingHours =
        match cuisine with
        | Turkish -> PlaceOpeningHours.AlwaysOpen
        | _ -> OpeningHours.restaurantOpeningHours

    (openingHours, place)
    ||> World.Place.changeOpeningHours
    |> World.Place.addExit Ids.Common.restaurant streetId

/// Creates a studio with the given name, quality, price per song and zone.
let createStudio streetId (name, quality, pricePerSong, producer, zoneId) =
    let studio =
        { Producer = producer
          PricePerSong = pricePerSong }

    World.Place.create name quality (Studio studio) Layouts.studioLayout zoneId
    |> World.Place.changeOpeningHours OpeningHours.servicesOpeningHours
    |> World.Place.addExit Ids.Studio.masteringRoom streetId

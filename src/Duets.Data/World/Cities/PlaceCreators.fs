module Duets.Data.World.Cities.PlaceCreators

open Duets.Data.World
open Duets.Entities

/// Creates a bar with the given name, quality and zone.
let createBar (name, quality, zone) =
    World.Place.create name quality Bar Layouts.barRoomLayout zone
    |> World.Place.changeOpeningHours OpeningHours.barOpeningHours

/// Creates a bookstore with the given name, quality and zone.
let createBookstore (name, quality, zone) =
    World.Place.create name quality Bookstore Layouts.bookstoreLayout zone
    |> World.Place.changeOpeningHours OpeningHours.servicesOpeningHours

/// Creates a cafe with the given name, quality and zone.
let createCafe (name, quality, zone) =
    World.Place.create name quality Cafe Layouts.cafeRoomLayout zone
    |> World.Place.changeOpeningHours OpeningHours.cafeOpeningHours

/// Creates a casino with the given name, quality and zone.
let createCasino (name, quality, zone) =
    World.Place.create name quality Casino Layouts.casinoLayout zone

/// Creates a concert space with the given name, capacity, quality and zone.
let createConcertSpace (name, capacity, zone, quality, layout) =
    World.Place.create
        name
        quality
        (ConcertSpace { Capacity = capacity })
        layout
        zone
    |> World.Place.changeOpeningHours OpeningHours.concertSpaceOpeningHours

/// Creates a gym with the given name, quality and zone.
let createGym (city: City) (name, quality, zone) =
    let place = World.Place.create name quality Gym Layouts.gymLayout zone

    let entranceChip = Item.Key.createGymChipFor city.Id place.Id

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
    |> World.Place.changeOpeningHours OpeningHours.gymOpeningHours

/// Creates a hotel with the given name, quality, price per night and zone.
let createHotel (name, quality, pricePerNight, zone) =
    World.Place.create
        name
        quality
        (Hotel { PricePerNight = pricePerNight })
        Layouts.hotelLayout
        zone

/// Creates a merchandise workshop with the given name and zone.
let createMerchandiseWorkshop (name, zone) =
    World.Place.create
        name
        100<quality>
        MerchandiseWorkshop
        Layouts.merchandiseWorkshopLayout
        zone

/// Creates a rehearsal space with the given name, quality, price and zone.
let createRehearsalSpace (name, quality, price, zone) =
    World.Place.create
        name
        quality
        (RehearsalSpace { Price = price })
        Layouts.rehearsalSpaceLayout
        zone
    |> World.Place.changeOpeningHours OpeningHours.servicesOpeningHours

/// Creates a radio studio with the given name, quality, music genre and zone.
let createRadioStudio (city: City) (name, quality, musicGenre, zone) =
    let place =
        World.Place.create
            name
            quality
            (RadioStudio { MusicGenre = musicGenre })
            Layouts.radioStudioLayout
            zone

    let invitation = Item.Key.createEntranceCardFor city.Id place.Id

    place
    |> World.Place.changeRoom Ids.RadioStudio.recordingRoom (function
        | Some room ->
            let requiredItems =
                { ComingFrom = Ids.RadioStudio.lobby
                  Items = [ invitation ] }

            room
            |> World.Room.changeRequiredItemForEntrance requiredItems
            |> Some
        | _ -> None)
    |> World.Place.changeOpeningHours OpeningHours.servicesOpeningHours

/// Creates a restaurant with the given name, quality, cuisine and zone.
let createRestaurant (name, quality, cuisine, zone) =
    let place =
        World.Place.create
            name
            quality
            Restaurant
            (Layouts.restaurantRoomLayout cuisine)
            zone

    let openingHours =
        match cuisine with
        | Turkish -> PlaceOpeningHours.AlwaysOpen
        | _ -> OpeningHours.restaurantOpeningHours

    (openingHours, place) ||> World.Place.changeOpeningHours

/// Creates a studio with the given name, quality, price per song and zone.
let createStudio (name, quality, pricePerSong, zone, producer) =
    let studio =
        { Producer = producer
          PricePerSong = pricePerSong }

    World.Place.create name quality (Studio studio) Layouts.studioLayout zone
    |> World.Place.changeOpeningHours OpeningHours.servicesOpeningHours

module Duets.Data.World.Cities.PlaceCreators

open Duets.Data.World
open Duets.Entities

/// Creates a bar with the given name, quality and zone.
let createBar streetId (name, quality, zoneId) =
    World.Place.create name quality Bar Layouts.barRoomLayout zoneId streetId
    |> World.Place.changeOpeningHours OpeningHours.barOpeningHours
    |> World.Place.addExit Ids.Common.bar streetId

/// Creates a bookstore with the given name, quality and zone.
let createBookstore streetId (name, quality, zoneId) =
    World.Place.create
        name
        quality
        Bookstore
        Layouts.bookstoreLayout
        zoneId
        streetId
    |> World.Place.changeOpeningHours OpeningHours.servicesOpeningHours
    |> World.Place.addExit Ids.Bookstore.readingRoom streetId

/// Creates a cafe with the given name, quality and zone.
let createCafe streetId (name, quality, zoneId) =
    World.Place.create name quality Cafe Layouts.cafeRoomLayout zoneId streetId
    |> World.Place.changeOpeningHours OpeningHours.cafeOpeningHours
    |> World.Place.addExit Ids.Common.cafe streetId

/// Creates a car dealer with the given name, quality, cone and car price range.
let createCarDealer streetId (name, quality, zoneId, carDealer) =
    World.Place.create
        name
        quality
        (CarDealer(carDealer))
        Layouts.carDealerLayout
        zoneId
        streetId
    |> World.Place.changeOpeningHours OpeningHours.servicesOpeningHours
    |> World.Place.addExit Ids.CarDealer.showRoom streetId

/// Creates a casino with the given name, quality and zone.
let createCasino streetId (name, quality, zoneId) =
    World.Place.create name quality Casino Layouts.casinoLayout zoneId streetId
    |> World.Place.addExit Ids.Common.lobby streetId

/// Creates a cinema with the given name, quality and zone.
let createCinema (cityId: CityId) streetId (name, quality, zoneId) =
    let place =
        World.Place.create name quality Cinema Layouts.cinemaLayout zoneId streetId

    let cinemaTicket = Item.Key.createCinemaTicketFor cityId place.Id

    place
    |> World.Place.changeRoom Ids.Cinema.screeningRoom (function
        | Some room ->
            let requiredItems =
                { ComingFrom = Ids.Common.lobby
                  Items = [ cinemaTicket ] }

            room
            |> World.Room.changeRequiredItemForEntrance requiredItems
            |> Some
        | _ -> None)
    |> World.Place.changeOpeningHours OpeningHours.cinemaOpeningHours
    |> World.Place.addExit Ids.Common.lobby streetId

/// Creates a concert space with the given name, capacity, quality and zone.
let createConcertSpace streetId (name, capacity, quality, layout, zoneId) =
    World.Place.create
        name
        quality
        (ConcertSpace { Capacity = capacity })
        layout
        zoneId
        streetId
    |> World.Place.changeOpeningHours OpeningHours.concertSpaceOpeningHours
    |> World.Place.addExit Ids.Common.lobby streetId

/// Creates a gym with the given name, quality and zone.
let createGym (city: City) streetId (name, quality, zoneId) =
    let place =
        World.Place.create name quality Gym Layouts.gymLayout zoneId streetId

    let entranceChip = Item.Key.createGymChipFor city.Id place.Id

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
    World.Place.create
        "Home"
        100<quality>
        Home
        Layouts.homeLayout
        zoneId
        streetId
    |> World.Place.addExit Ids.Home.kitchen streetId

/// Creates a hotel with the given name, quality, price per night and zone.
let createHotel streetId (name, quality, pricePerNight, zoneId) =
    World.Place.create
        name
        quality
        (Hotel { PricePerNight = pricePerNight })
        Layouts.hotelLayout
        zoneId
        streetId
    |> World.Place.addExit Ids.Common.lobby streetId

/// Creates a hospital with the given name, quality and zone.
let createHospital streetId (name, quality, zoneId) =
    World.Place.create
        name
        quality
        Hospital
        Layouts.hospitalLayout
        zoneId
        streetId
    |> World.Place.addExit Ids.Common.lobby streetId

/// Creates a merchandise workshop with the given name and zone.
let createMerchandiseWorkshop streetId (name, zoneId) =
    World.Place.create
        name
        100<quality>
        MerchandiseWorkshop
        Layouts.merchandiseWorkshopLayout
        zoneId
        streetId
    |> World.Place.changeOpeningHours OpeningHours.servicesOpeningHours
    |> World.Place.addExit Ids.Workshop.workshop streetId

/// Creates a metro station with the given name and lines.
let createMetro streetId (name, zoneId) =
    World.Place.create
        name
        100<quality>
        MetroStation
        Layouts.metroLayout
        zoneId
        streetId
    |> World.Place.addExit Ids.Metro.platform streetId

/// Creates a rehearsal space with the given name, quality, price and zone.
let createRehearsalSpace streetId (name, quality, price, zoneId) =
    World.Place.create
        name
        quality
        (RehearsalSpace { Price = price })
        Layouts.rehearsalSpaceLayout
        zoneId
        streetId
    |> World.Place.changeOpeningHours OpeningHours.servicesOpeningHours
    |> World.Place.addExit Ids.Common.lobby streetId

/// Creates a radio studio with the given name, quality, music genre and zone.
let createRadioStudio
    (city: City)
    streetId
    (name, quality, musicGenre, zoneId)
    =
    let place =
        World.Place.create
            name
            quality
            (RadioStudio { MusicGenre = musicGenre })
            Layouts.radioStudioLayout
            zoneId
            streetId

    let invitation = Item.Key.createEntranceCardFor city.Id place.Id

    place
    |> World.Place.changeRoom Ids.Studio.recordingRoom (function
        | Some room ->
            let requiredItems =
                { ComingFrom = Ids.Common.lobby
                  Items = [ invitation ] }

            room
            |> World.Room.changeRequiredItemForEntrance requiredItems
            |> Some
        | _ -> None)
    |> World.Place.changeOpeningHours OpeningHours.radioStudioOpeningHours
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
            streetId

    let openingHours =
        match cuisine with
        | Turkish -> PlaceOpeningHours.AlwaysOpen
        | _ -> OpeningHours.restaurantOpeningHours

    World.Place.changeOpeningHours openingHours place
    |> World.Place.addExit Ids.Common.restaurant streetId

/// Creates a studio with the given name, quality, price per song and zone.
let createStudio streetId (name, quality, pricePerSong, producer, zoneId) =
    let studio =
        { Producer = producer
          PricePerSong = pricePerSong }

    World.Place.create
        name
        quality
        (Studio studio)
        Layouts.studioLayout
        zoneId
        streetId
    |> World.Place.changeOpeningHours OpeningHours.servicesOpeningHours
    |> World.Place.addExit Ids.Studio.masteringRoom streetId

/// Creates an airport with the given name and quality.
let createAirport streetId (name, quality, zoneId) =
    World.Place.create
        name
        quality
        Airport
        Layouts.airportLayout
        zoneId
        streetId
    |> World.Place.addExit Ids.Common.lobby streetId

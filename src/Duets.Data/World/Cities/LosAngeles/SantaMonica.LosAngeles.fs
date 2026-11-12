module Duets.Data.World.Cities.LosAngeles.SantaMonica

open Duets.Data.World.Cities.LosAngeles
open Duets.Data.World.Cities
open Duets.Entities
open Duets.Entities.Calendar

let oceanAvenue (zone: Zone) =
    let street =
        World.Street.create Ids.Street.oceanAvenue (StreetType.Split(North, 2))
        |> World.Street.tag "waterfront"
        |> World.Street.tag "santa-monica-pier"

    let concertSpaces =
        [ ("Santa Monica Pier",
           5000,
           89<quality>,
           Layouts.concertSpaceLayout4,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let restaurants =
        [ ("Water Grill Santa Monica", 90<quality>, American, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let hotels =
        [ ("Hotel Casa del Mar", 95<quality>, 500m<dd>, zone.Id)
          ("Fairmont Miramar Hotel & Bungalows", 93<quality>, 450m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    let bars =
        [ ("Chez Jay", 86<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let metroStation =
        ("Downtown Santa Monica", zone.Id)
        |> (PlaceCreators.createMetro street.Id)

    let street =
        street
        |> World.Street.addPlaces concertSpaces
        |> World.Street.addPlaces restaurants
        |> World.Street.addPlaces hotels
        |> World.Street.addPlaces bars
        |> World.Street.addPlace metroStation

    street, metroStation

let mainStreet (zone: Zone) =
    let street =
        World.Street.create Ids.Street.mainStreet StreetType.OneWay

    let concertSpaces =
        [ ("Harvelle's",
           200,
           86<quality>,
           Layouts.concertSpaceLayout1,
           zone.Id) ]
        |> List.map (
            PlaceCreators.createConcertSpace street.Id
            >> World.Place.tag "jazz-club"
        )

    let bookstores =
        [ ("Angel City Books & Records", 86<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBookstore street.Id)

    let cafes =
        [ ("Urth Caffé", 87<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    let merchandiseWorkshops =
        [ ("Truetone Music", zone.Id) ]
        |> List.map (PlaceCreators.createMerchandiseWorkshop street.Id)

    street
    |> World.Street.addPlaces concertSpaces
    |> World.Street.addPlaces bookstores
    |> World.Street.addPlaces cafes
    |> World.Street.addPlaces merchandiseWorkshops

let picoBoulevard (zone: Zone) (city: City) =
    let street =
        World.Street.create Ids.Street.picoBoulevard (StreetType.Split(East, 2))

    let home = PlaceCreators.createHome street.Id zone.Id

    let carDealers =
        [ ("Pacific Coast Motors",
           82<quality>,
           zone.Id,
           { Dealer =
               (Character.from
                   "Jennifer Martinez"
                   Female
                   (Shorthands.Spring 15<days> 1980<years>))
             PriceRange = CarPriceRange.MidRange }) ]
        |> List.map (PlaceCreators.createCarDealer street.Id)

    let rehearsalSpaces =
        [ ("The Lockout Rooms", 84<quality>, 350m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createRehearsalSpace street.Id)

    let studios =
        [ ("The Garage Studio",
           82<quality>,
           400m<dd>,
           (Character.from
               "Sarah Mitchell"
               Female
               (Shorthands.Autumn 12<days> 1988<years>)),
           zone.Id) ]
        |> List.map (PlaceCreators.createStudio street.Id)

    let hospitals =
        [ ("UCLA Santa Monica Medical Center", 92<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createHospital street.Id)

    let gyms =
        [ ("Equinox Santa Monica", 88<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createGym city street.Id)

    let concertSpaces =
        [ ("McCabe's Guitar Shop",
           150,
           88<quality>,
           Layouts.concertSpaceLayout1,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id >> World.Place.tag "seated")

    street
    |> World.Street.addPlace home
    |> World.Street.addPlaces concertSpaces
    |> World.Street.addPlaces carDealers
    |> World.Street.addPlaces rehearsalSpaces
    |> World.Street.addPlaces studios
    |> World.Street.addPlaces hospitals
    |> World.Street.addPlaces gyms

let createZone (city: City) =
    let santaMonicaZone = World.Zone.create Ids.Zone.santaMonica

    let oceanAvenue, oceanAvenueStation = oceanAvenue santaMonicaZone
    let mainStreet = mainStreet santaMonicaZone
    let picoBoulevard = picoBoulevard santaMonicaZone city

    let station =
        { Lines = [ Blue ]
          LeavesToStreet = oceanAvenue.Id
          PlaceId = oceanAvenueStation.Id }

    santaMonicaZone
    |> World.Zone.addStreet (World.Node.create oceanAvenue.Id oceanAvenue)
    |> World.Zone.addStreet (World.Node.create mainStreet.Id mainStreet)
    |> World.Zone.addStreet (World.Node.create picoBoulevard.Id picoBoulevard)
    |> World.Zone.connectStreets oceanAvenue.Id mainStreet.Id East
    |> World.Zone.connectStreets mainStreet.Id picoBoulevard.Id East
    |> World.Zone.addMetroStation station

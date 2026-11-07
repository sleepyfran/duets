module Duets.Data.World.Cities.Tokyo.Shibuya

open Duets.Data.World.Cities.Tokyo
open Duets.Data.World.Cities
open Duets.Entities
open Duets.Entities.Calendar

let dogenzaka (zone: Zone) =
    let street =
        World.Street.create Ids.Street.dogenzaka (StreetType.Split(East, 2))

    let concertSpaces =
        [ ("Club Quattro", 800, 90<quality>, Layouts.concertSpaceLayout3, zone.Id)
          ("WWW", 400, 88<quality>, Layouts.concertSpaceLayout2, zone.Id)
          ("Shibuya O-East", 1300, 89<quality>, Layouts.concertSpaceLayout3, zone.Id)
          ("Shibuya O-West", 500, 86<quality>, Layouts.concertSpaceLayout2, zone.Id)
          ("The Milky Way", 150, 82<quality>, Layouts.concertSpaceLayout1, zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let bars =
        [ ("Tight Bar", 85<quality>, zone.Id)
          ("Bar Martha", 83<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let hotels =
        [ ("Shibuya Stream Excel Hotel Tokyu", 88<quality>, 350m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    let bookstores =
        [ ("Tower Records Shibuya", 91<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBookstore street.Id)

    let metroStation =
        ("Shibuya Station", zone.Id) |> PlaceCreators.createMetro street.Id

    let street =
        street
        |> World.Street.addPlaces concertSpaces
        |> World.Street.addPlaces bars
        |> World.Street.addPlaces hotels
        |> World.Street.addPlaces bookstores
        |> World.Street.addPlace metroStation

    street, metroStation

let centerGai (cityId: CityId) (zone: Zone) =
    let street =
        World.Street.create Ids.Street.centerGai StreetType.OneWay
        |> World.Street.tag "pedestrian"

    let cafes =
        [ ("Starbucks Reserve Roastery Tokyo", 92<quality>, zone.Id)
          ("Fuglen Tokyo", 90<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    let restaurants =
        [ ("Ichiran Ramen Shibuya", 90<quality>, Japanese, zone.Id)
          ("Genki Sushi", 84<quality>, Japanese, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let merchandiseWorkshops =
        [ ("Recofan Shibuya", zone.Id) ]
        |> List.map (PlaceCreators.createMerchandiseWorkshop street.Id)

    let cinemas =
        [ ("Shibuya Cinemart", 87<quality>, zone.Id)
          ("109 Cinemas Shibuya", 89<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCinema cityId street.Id)

    street
    |> World.Street.addPlaces cafes
    |> World.Street.addPlaces restaurants
    |> World.Street.addPlaces merchandiseWorkshops
    |> World.Street.addPlaces cinemas

let miyamasuzaka (zone: Zone) (city: City) =
    let street =
        World.Street.create Ids.Street.miyamasuzaka StreetType.OneWay

    let studios =
        [ ("Shibuya Recording Studio",
           88<quality>,
           600m<dd>,
           (Character.from
               "Kenji Watanabe"
               Male
               (Shorthands.Spring 10<days> 1975<years>)),
           zone.Id) ]
        |> List.map (PlaceCreators.createStudio street.Id)

    let rehearsalSpaces =
        [ ("Beat Station Rehearsal", 86<quality>, 350m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createRehearsalSpace street.Id)

    let home = PlaceCreators.createHome street.Id zone.Id

    let carDealers =
        [ ("Toyota Mobility Shibuya",
           85<quality>,
           zone.Id,
           { Dealer =
               (Character.from
                   "Yuki Tanaka"
                   Female
                   (Shorthands.Summer 22<days> 1984<years>))
             PriceRange = CarPriceRange.MidRange }) ]
        |> List.map (PlaceCreators.createCarDealer street.Id)

    street
    |> World.Street.addPlace home
    |> World.Street.addPlaces studios
    |> World.Street.addPlaces rehearsalSpaces
    |> World.Street.addPlaces carDealers

let createZone (city: City) =
    let shibuyaZone = World.Zone.create Ids.Zone.shibuya

    let dogenzaka, shibuyaMetroStation = dogenzaka shibuyaZone
    let centerGai = centerGai city.Id shibuyaZone
    let miyamasuzaka = miyamasuzaka shibuyaZone city

    let station =
        { Lines = [ Red ]
          LeavesToStreet = dogenzaka.Id
          PlaceId = shibuyaMetroStation.Id }

    shibuyaZone
    |> World.Zone.addStreet (World.Node.create dogenzaka.Id dogenzaka)
    |> World.Zone.addStreet (World.Node.create centerGai.Id centerGai)
    |> World.Zone.addStreet (World.Node.create miyamasuzaka.Id miyamasuzaka)
    |> World.Zone.connectStreets dogenzaka.Id centerGai.Id North
    |> World.Zone.connectStreets centerGai.Id miyamasuzaka.Id East
    |> World.Zone.addMetroStation station

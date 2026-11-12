module Duets.Data.World.Cities.Seoul.Hongdae

open Duets.Data.World.Cities.Seoul
open Duets.Data.World.Cities
open Duets.Entities
open Duets.Entities.Calendar

let hongikRo (zone: Zone) =
    let street =
        World.Street.create Ids.Street.hongikRo (StreetType.Split(East, 2))
        |> World.Street.tag "murals"
        |> World.Street.tag "hongdae-busking"

    let concertSpaces =
        [ ("Rolling Hall", 300, 87<quality>, Layouts.concertSpaceLayout2, zone.Id)
          ("Club FF", 150, 84<quality>, Layouts.concertSpaceLayout1, zone.Id)
          ("Club Gogos", 200, 83<quality>, Layouts.concertSpaceLayout1, zone.Id)
          ("MUV Hall", 400, 86<quality>, Layouts.concertSpaceLayout2, zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let bars =
        [ ("Thursday Party", 85<quality>, zone.Id)
          ("Zen Bar", 83<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let metroStation =
        ("Hongik University Station", zone.Id)
        |> PlaceCreators.createMetro street.Id

    let street =
        street
        |> World.Street.addPlaces concertSpaces
        |> World.Street.addPlaces bars
        |> World.Street.addPlace metroStation

    street, metroStation

let wausanRo (zone: Zone) (city: City) =
    let street =
        World.Street.create Ids.Street.wausanRo StreetType.OneWay

    let studios =
        [ ("Soundholick Studio",
           87<quality>,
           550m<dd>,
           (Character.from
               "Min-jun Lee"
               Male
               (Shorthands.Winter 3<days> 1979<years>)),
           zone.Id) ]
        |> List.map (PlaceCreators.createStudio street.Id)

    let rehearsalSpaces =
        [ ("Velvet Underground Practice Rooms", 85<quality>, 300m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createRehearsalSpace street.Id)

    let home = PlaceCreators.createHome street.Id zone.Id

    let carDealers =
        [ ("Kia Hongdae",
           82<quality>,
           zone.Id,
           { Dealer =
               (Character.from
                   "Seo-yeon Park"
                   Female
                   (Shorthands.Spring 20<days> 1988<years>))
             PriceRange = CarPriceRange.Budget }) ]
        |> List.map (PlaceCreators.createCarDealer street.Id)

    let merchandiseWorkshops =
        [ ("Vinyl & Plastic Records", zone.Id) ]
        |> List.map (PlaceCreators.createMerchandiseWorkshop street.Id)

    street
    |> World.Street.addPlace home
    |> World.Street.addPlaces studios
    |> World.Street.addPlaces rehearsalSpaces
    |> World.Street.addPlaces carDealers
    |> World.Street.addPlaces merchandiseWorkshops

let yanghwaRo (cityId: CityId) (zone: Zone) =
    let street =
        World.Street.create Ids.Street.yangwhaRo (StreetType.Split(West, 2))

    let concertSpaces =
        [ ("Mecenatpolis Concert Hall", 800, 87<quality>, Layouts.concertSpaceLayout3, zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id >> World.Place.tag "seated")

    let restaurants =
        [ ("Tosokchon Samgyetang", 90<quality>, Korean, zone.Id)
          ("Mapo Galmaegi", 86<quality>, Korean, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let cafes =
        [ ("Anthracite Coffee Hongdae", 92<quality>, zone.Id)
          ("Fritz Coffee Company", 91<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    let cinemas =
        [ ("CGV Hongdae", 88<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCinema cityId street.Id)

    let merchandiseWorkshops =
        [ ("Kakao Friends Store Hongdae", zone.Id) ]
        |> List.map (PlaceCreators.createMerchandiseWorkshop street.Id)

    street
    |> World.Street.addPlaces concertSpaces
    |> World.Street.addPlaces restaurants
    |> World.Street.addPlaces cafes
    |> World.Street.addPlaces cinemas
    |> World.Street.addPlaces merchandiseWorkshops

let createZone (city: City) =
    let hongdaeZone = World.Zone.create Ids.Zone.hongdae

    let hongikRo, hongdaeMetroStation = hongikRo hongdaeZone
    let wausanRo = wausanRo hongdaeZone city
    let yanghwaRo = yanghwaRo city.Id hongdaeZone

    let station =
        { Lines = [ Red ]
          LeavesToStreet = hongikRo.Id
          PlaceId = hongdaeMetroStation.Id }

    hongdaeZone
    |> World.Zone.addStreet (World.Node.create hongikRo.Id hongikRo)
    |> World.Zone.addStreet (World.Node.create wausanRo.Id wausanRo)
    |> World.Zone.addStreet (World.Node.create yanghwaRo.Id yanghwaRo)
    |> World.Zone.connectStreets hongikRo.Id wausanRo.Id North
    |> World.Zone.connectStreets wausanRo.Id yanghwaRo.Id West
    |> World.Zone.addMetroStation station

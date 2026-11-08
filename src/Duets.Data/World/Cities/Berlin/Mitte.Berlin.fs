module Duets.Data.World.Cities.Berlin.Mitte

open Duets.Data.World.Cities.Berlin
open Duets.Data.World.Cities
open Duets.Entities
open Duets.Entities.Calendar

let unterDenLinden (cityId: CityId) (zone: Zone) =
    let street =
        World.Street.create
            Ids.Street.unterDenLinden
            (StreetType.Split(East, 2))
        |> World.Street.tag "brandenburg-gate"

    let concertSpaces =
        [ ("Staatsoper Unter den Linden",
           1396,
           96<quality>,
           Layouts.concertSpaceLayout3,
           zone.Id)
          ("Konzerthaus Berlin",
           1400,
           94<quality>,
           Layouts.concertSpaceLayout3,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id >> World.Place.tag "seated")

    let restaurants =
        [ ("Borchardt", 95<quality>, German, zone.Id)
          ("Café Einstein Stammhaus", 90<quality>, German, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let hotels =
        [ ("Hotel Adlon Kempinski", 98<quality>, 700m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    let cinemas =
        [ ("Kino International", 90<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCinema cityId street.Id)

    let metroStation =
        ("Brandenburger Tor", zone.Id)
        |> (PlaceCreators.createMetro street.Id)

    let street =
        street
        |> World.Street.addPlaces concertSpaces
        |> World.Street.addPlaces restaurants
        |> World.Street.addPlaces hotels
        |> World.Street.addPlaces cinemas
        |> World.Street.addPlace metroStation

    street, metroStation

let alexanderplatz (zone: Zone) =
    let street =
        World.Street.create
            Ids.Street.alexanderplatz
            (StreetType.Split(North, 2))
        |> World.Street.tag "alexanderplatz"

    let bars =
        [ ("Park Inn Rooftop Bar", 88<quality>, zone.Id)
          |> PlaceCreators.createBar street.Id
          |> World.Place.tag "cocktail-bar"
          ("The Alex Pub", 82<quality>, zone.Id)
          |> PlaceCreators.createBar street.Id
          |> World.Place.tag "pub" ]

    let cafes =
        [ ("Café Oliv", 85<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    let bookstores =
        [ ("Hugendubel Berlin", 88<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBookstore street.Id)

    let merchandiseWorkshops =
        [ ("Saturn Electronics", zone.Id) ]
        |> List.map (PlaceCreators.createMerchandiseWorkshop street.Id)

    street
    |> World.Street.addPlaces bars
    |> World.Street.addPlaces cafes
    |> World.Street.addPlaces bookstores
    |> World.Street.addPlaces merchandiseWorkshops

let friedrichstrasse (zone: Zone) =
    let street =
        World.Street.create
            Ids.Street.friedrichstrasse
            StreetType.OneWay
        |> World.Street.tag "checkpoint-charlie"

    let concertSpaces =
        [ ("Friedrichstadt-Palast",
           1895,
           93<quality>,
           Layouts.concertSpaceLayout3,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id >> World.Place.tag "seated")

    let casinos =
        [ ("Spielbank Berlin", 90<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCasino street.Id)

    let carDealers =
        [ ("Mercedes-Benz Gallery Berlin",
           97<quality>,
           zone.Id,
           { Dealer =
               (Character.from
                   "Klaus Müller"
                   Male
                   (Shorthands.Spring 5<days> 1971<years>))
             PriceRange = CarPriceRange.Premium }) ]
        |> List.map (PlaceCreators.createCarDealer street.Id)

    street
    |> World.Street.addPlaces concertSpaces
    |> World.Street.addPlaces casinos
    |> World.Street.addPlaces carDealers

let createZone (cityId: CityId) =
    let mitteZone = World.Zone.create Ids.Zone.mitte

    let unterDenLinden, unterDenLindenStation = unterDenLinden cityId mitteZone
    let alexanderplatz = alexanderplatz mitteZone
    let friedrichstrasse = friedrichstrasse mitteZone

    let station =
        { Lines = [ Red; Blue ]
          LeavesToStreet = unterDenLinden.Id
          PlaceId = unterDenLindenStation.Id }

    mitteZone
    |> World.Zone.addStreet (World.Node.create unterDenLinden.Id unterDenLinden)
    |> World.Zone.addStreet (World.Node.create alexanderplatz.Id alexanderplatz)
    |> World.Zone.addStreet (World.Node.create friedrichstrasse.Id friedrichstrasse)
    |> World.Zone.connectStreets unterDenLinden.Id alexanderplatz.Id East
    |> World.Zone.connectStreets alexanderplatz.Id friedrichstrasse.Id North
    |> World.Zone.addMetroStation station

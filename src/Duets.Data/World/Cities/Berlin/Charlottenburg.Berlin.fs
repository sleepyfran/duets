module Duets.Data.World.Cities.Berlin.Charlottenburg

open Duets.Data.World.Cities.Berlin
open Duets.Data.World.Cities
open Duets.Entities
open Duets.Entities.Calendar

let kurfuerstendamm (cityId: CityId) (zone: Zone) =
    let street =
        World.Street.create
            Ids.Street.kurfuerstendamm
            (StreetType.Split(East, 3))

    let concertSpaces =
        [ ("Theater des Westens",
           1637,
           93<quality>,
           Layouts.concertSpaceLayout3,
           zone.Id)
          ("Schaubühne am Lehniner Platz",
           750,
           95<quality>,
           Layouts.concertSpaceLayout2,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id >> World.Place.tag "seated")

    let hotels =
        [ ("Hotel Zoo Berlin", 92<quality>, 320m<dd>, zone.Id)
          ("Waldorf Astoria Berlin", 98<quality>, 650m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    let cinemas =
        [ ("Zoo Palast", 92<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCinema cityId street.Id)

    let metroStation =
        ("U Kurfürstendamm Station", zone.Id)
        |> (PlaceCreators.createMetro street.Id)

    let street =
        street
        |> World.Street.addPlaces concertSpaces
        |> World.Street.addPlaces hotels
        |> World.Street.addPlaces cinemas
        |> World.Street.addPlace metroStation

    street, metroStation

let kantstrasse (zone: Zone) =
    let street =
        World.Street.create
            Ids.Street.kantstrasse
            StreetType.OneWay

    let restaurants =
        [ ("Marjellchen", 88<quality>, German, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let cafes =
        [ ("Café Literaturhaus", 90<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    let studios =
        [ ("Hansa by the Wall",
           92<quality>,
           550m<dd>,
           (Character.from
               "Ingrid Hoffmann"
               Female
               (Shorthands.Winter 3<days> 1975<years>)),
           zone.Id) ]
        |> List.map (PlaceCreators.createStudio street.Id)

    let rehearsalSpaces =
        [ ("Charlottenburg Probe", 86<quality>, 320m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createRehearsalSpace street.Id)

    street
    |> World.Street.addPlaces restaurants
    |> World.Street.addPlaces cafes
    |> World.Street.addPlaces studios
    |> World.Street.addPlaces rehearsalSpaces

let bismarckstrasse (zone: Zone) (city: City) =
    let street =
        World.Street.create
            Ids.Street.bismarckstrasse
            StreetType.OneWay

    let concertSpaces =
        [ ("Deutsche Oper Berlin",
           1865,
           96<quality>,
           Layouts.concertSpaceLayout3,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id >> World.Place.tag "seated")

    let gyms =
        [ ("McFit Charlottenburg", 80<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createGym city street.Id)

    let radioStudios =
        [ ("RBB Radio Berlin", 89<quality>, "Pop", zone.Id) ]
        |> List.map (PlaceCreators.createRadioStudio city street.Id)

    let carDealers =
        [ ("BMW Berlin Showroom",
           90<quality>,
           zone.Id,
           { Dealer =
               (Character.from
                   "Petra Schneider"
                   Female
                   (Shorthands.Spring 18<days> 1978<years>))
             PriceRange = CarPriceRange.MidRange }) ]
        |> List.map (PlaceCreators.createCarDealer street.Id)

    street
    |> World.Street.addPlaces concertSpaces
    |> World.Street.addPlaces gyms
    |> World.Street.addPlaces radioStudios
    |> World.Street.addPlaces carDealers

let createZone (cityId: CityId) (city: City) =
    let charlottenburgZone = World.Zone.create Ids.Zone.charlottenburg

    let kurfuerstendamm, kurfuerstendammStation = kurfuerstendamm cityId charlottenburgZone
    let kantstrasse = kantstrasse charlottenburgZone
    let bismarckstrasse = bismarckstrasse charlottenburgZone city

    let station =
        { Lines = [ Red ]
          LeavesToStreet = kurfuerstendamm.Id
          PlaceId = kurfuerstendammStation.Id }

    charlottenburgZone
    |> World.Zone.addStreet (World.Node.create kurfuerstendamm.Id kurfuerstendamm)
    |> World.Zone.addStreet (World.Node.create kantstrasse.Id kantstrasse)
    |> World.Zone.addStreet (World.Node.create bismarckstrasse.Id bismarckstrasse)
    |> World.Zone.connectStreets kurfuerstendamm.Id kantstrasse.Id North
    |> World.Zone.connectStreets kantstrasse.Id bismarckstrasse.Id East
    |> World.Zone.addMetroStation station

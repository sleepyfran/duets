module Duets.Data.World.Cities.Berlin.Kreuzberg

open Duets.Data.World.Cities.Berlin
open Duets.Data.World.Cities
open Duets.Entities
open Duets.Entities.Calendar

let oranienstrasse (zone: Zone) =
    let street =
        World.Street.create
            Ids.Street.oranienstrasse
            (StreetType.Split(East, 2))
        |> World.Street.tag "murals"

    let concertSpaces =
        [ ("SO36",
           700,
           89<quality>,
           Layouts.concertSpaceLayout2,
           zone.Id)
          ("Lido",
           600,
           87<quality>,
           Layouts.concertSpaceLayout2,
           zone.Id)
          ("Privatclub",
           400,
           84<quality>,
           Layouts.concertSpaceLayout2,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let bars =
        [ ("Möbel Olfe", 86<quality>, zone.Id)
          ("Zum Goldenen Hahn", 80<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let studios =
        [ ("Hansa Studios",
           95<quality>,
           600m<dd>,
           (Character.from
               "Thomas Köhler"
               Male
               (Shorthands.Autumn 10<days> 1967<years>)),
           zone.Id) ]
        |> List.map (PlaceCreators.createStudio street.Id)

    let cafes =
        [ ("Café Morgenrot", 83<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    street
    |> World.Street.addPlaces concertSpaces
    |> World.Street.addPlaces bars
    |> World.Street.addPlaces studios
    |> World.Street.addPlaces cafes

let kottbusserDamm (zone: Zone) (city: City) =
    let street =
        World.Street.create
            Ids.Street.kottbusserDamm
            (StreetType.Split(North, 2))
        |> World.Street.tag "elevated-train"

    let home = PlaceCreators.createHome street.Id zone.Id

    let restaurants =
        [ ("Mustafa's Gemüse Kebap", 91<quality>, German, zone.Id)
          ("Hasir Restaurant", 88<quality>, Turkish, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let rehearsalSpaces =
        [ ("Kreuzberg Probe Rooms", 85<quality>, 280m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createRehearsalSpace street.Id)

    let carDealers =
        [ ("Kotti Motors",
           70<quality>,
           zone.Id,
           { Dealer =
               (Character.from
                   "Ali Yilmaz"
                   Male
                   (Shorthands.Summer 22<days> 1980<years>))
             PriceRange = CarPriceRange.Budget }) ]
        |> List.map (PlaceCreators.createCarDealer street.Id)

    let gyms =
        [ ("Kottbusser Fitness", 81<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createGym city street.Id)

    let metroStation =
        ("Kottbusser Tor", zone.Id)
        |> (PlaceCreators.createMetro street.Id)

    let street =
        street
        |> World.Street.addPlace home
        |> World.Street.addPlaces restaurants
        |> World.Street.addPlaces rehearsalSpaces
        |> World.Street.addPlaces carDealers
        |> World.Street.addPlaces gyms
        |> World.Street.addPlace metroStation

    street, metroStation

let bergmannstrasse (zone: Zone) =
    let street =
        World.Street.create
            Ids.Street.bergmannstrasse
            StreetType.OneWay

    let bookstores =
        [ ("Antiquariat Kisch & Co.", 87<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBookstore street.Id)

    let merchandiseWorkshops =
        [ ("Hard Wax Record Shop", zone.Id) ]
        |> List.map (PlaceCreators.createMerchandiseWorkshop street.Id)

    let hospitals =
        [ ("Urban Hospital (Urbankrankenhaus)", 88<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createHospital street.Id)

    street
    |> World.Street.addPlaces bookstores
    |> World.Street.addPlaces merchandiseWorkshops
    |> World.Street.addPlaces hospitals

let createZone (city: City) =
    let kreuzbergZone = World.Zone.create Ids.Zone.kreuzberg

    let oranienstrasse = oranienstrasse kreuzbergZone
    let kottbusserDamm, kottbusserStation = kottbusserDamm kreuzbergZone city
    let bergmannstrasse = bergmannstrasse kreuzbergZone

    let station =
        { Lines = [ Blue ]
          LeavesToStreet = kottbusserDamm.Id
          PlaceId = kottbusserStation.Id }

    kreuzbergZone
    |> World.Zone.addStreet (World.Node.create oranienstrasse.Id oranienstrasse)
    |> World.Zone.addStreet (World.Node.create kottbusserDamm.Id kottbusserDamm)
    |> World.Zone.addStreet (World.Node.create bergmannstrasse.Id bergmannstrasse)
    |> World.Zone.connectStreets oranienstrasse.Id kottbusserDamm.Id East
    |> World.Zone.connectStreets kottbusserDamm.Id bergmannstrasse.Id North
    |> World.Zone.addMetroStation station

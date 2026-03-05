module Duets.Data.World.Cities.Berlin.Schoenefeld

open Duets.Data.World.Cities.Berlin
open Duets.Data.World.Cities
open Duets.Entities

let airportBoulevard (zone: Zone) =
    let street =
        World.Street.create Ids.Street.airportBoulevard StreetType.OneWay
        |> World.Street.attachContext
            """
        The Airport Boulevard runs through the heart of Berlin Brandenburg Airport (BER),
        the long-awaited international gateway to the German capital. After years of
        notorious delays, BER finally opened in 2020, serving as the main hub for flights
        connecting Berlin to destinations across Europe and the world. The surrounding area
        is a modern transit landscape of terminals, car parks, and business hotels, with
        direct rail links running into central Berlin via the S-Bahn and regional trains.
"""

    let airports =
        [ ("Berlin Brandenburg Airport (BER)", 88<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createAirport street.Id)

    let hotels =
        [ ("Radisson Blu Hotel BER", 82<quality>, 180m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    let metroStation =
        ("BER Terminal 1", zone.Id)
        |> (PlaceCreators.createMetro street.Id)

    let street =
        street
        |> World.Street.addPlaces airports
        |> World.Street.addPlaces hotels
        |> World.Street.addPlace metroStation

    street, metroStation

let zone =
    let schoenefeldZone = World.Zone.create Ids.Zone.schoenefeld

    let airportBoulevard, airportStation = airportBoulevard schoenefeldZone

    let station =
        { Lines = [ Blue ]
          LeavesToStreet = airportBoulevard.Id
          PlaceId = airportStation.Id }

    schoenefeldZone
    |> World.Zone.addStreet (World.Node.create airportBoulevard.Id airportBoulevard)
    |> World.Zone.addMetroStation station

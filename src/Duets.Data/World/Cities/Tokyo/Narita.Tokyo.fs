module Duets.Data.World.Cities.Tokyo.Narita

open Duets.Data.World.Cities.Tokyo
open Duets.Data.World.Cities
open Duets.Entities

let airportRoad (zone: Zone) =
    let street =
        World.Street.create Ids.Street.naritaRoad StreetType.OneWay
        |> World.Street.attachContext
            """
        The Airport Access Road connects Narita International Airport to the
        surrounding area, lined with large airport hotels, transit hubs, and
        international car rental facilities. Narita Airport serves as the
        primary international gateway to Tokyo, handling tens of millions of
        passengers annually across its three terminals. The road has a calm,
        functional character compared to the frenetic energy of central Tokyo,
        offering travellers a brief decompression zone before diving into the
        metropolis.
"""

    let airports =
        [ ("Narita International Airport", 90<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createAirport street.Id)

    let hotels =
        [ ("ANA Crowne Plaza Narita", 85<quality>, 250m<dd>, zone.Id)
          ("Narita Excel Hotel Tokyu", 83<quality>, 220m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    let metroStation =
        ("Narita Airport Terminal 1", zone.Id)
        |> PlaceCreators.createMetro street.Id

    let street =
        street
        |> World.Street.addPlaces airports
        |> World.Street.addPlaces hotels
        |> World.Street.addPlace metroStation

    street, metroStation

let zone =
    let naritaZone = World.Zone.create Ids.Zone.narita

    let airportRoad, naritaMetroStation = airportRoad naritaZone

    let station =
        { Lines = [ Blue ]
          LeavesToStreet = airportRoad.Id
          PlaceId = naritaMetroStation.Id }

    naritaZone
    |> World.Zone.addStreet (World.Node.create airportRoad.Id airportRoad)
    |> World.Zone.addMetroStation station

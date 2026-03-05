module Duets.Data.World.Cities.Seoul.Incheon

open Duets.Data.World.Cities.Seoul
open Duets.Data.World.Cities
open Duets.Entities

let airportRo (zone: Zone) =
    let street =
        World.Street.create Ids.Street.airportRo StreetType.OneWay
        |> World.Street.attachContext
            """
        Airport-ro is the main arterial road connecting Incheon International
        Airport to the expressway network and the broader Seoul metropolitan
        area. Lined with large international hotels, transit hubs, and car
        rental facilities, this road serves as the primary gateway into South
        Korea for millions of travellers each year. Incheon Airport has
        consistently been rated one of the best airports in the world, renowned
        for its efficiency, cleanliness, and extensive amenities.
"""

    let airports =
        [ ("Incheon International Airport", 96<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createAirport street.Id)

    let hotels =
        [ ("Grand Hyatt Incheon", 88<quality>, 280m<dd>, zone.Id)
          ("Incheon Airport Marriott", 85<quality>, 250m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    let metroStation =
        ("Incheon Airport Terminal 1", zone.Id)
        |> PlaceCreators.createMetro street.Id

    let street =
        street
        |> World.Street.addPlaces airports
        |> World.Street.addPlaces hotels
        |> World.Street.addPlace metroStation

    street, metroStation

let zone =
    let incheonZone = World.Zone.create Ids.Zone.incheon

    let airportRo, incheonMetroStation = airportRo incheonZone

    let station =
        { Lines = [ Blue ]
          LeavesToStreet = airportRo.Id
          PlaceId = incheonMetroStation.Id }

    incheonZone
    |> World.Zone.addStreet (World.Node.create airportRo.Id airportRo)
    |> World.Zone.addMetroStation station

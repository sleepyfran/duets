module Duets.Data.World.Cities.Paris.Roissy

open Duets.Data.World.Cities.Paris
open Duets.Data.World.Cities
open Duets.Entities

let avenueCharlesDeGaulle (zone: Zone) =
    let street =
        World.Street.create Ids.Street.avenueCharlesDeGaulle StreetType.OneWay
        |> World.Street.attachContext
            """
        Avenue Charles de Gaulle leads to and from Charles de Gaulle Airport,
        the largest international airport in France and the second busiest in
        Europe. Named after the French general and statesman, the avenue is flanked
        by a succession of large airport hotels, car rental agencies, logistics hubs
        and transport infrastructure. The RER B line runs directly beneath the
        avenue, providing a direct rail link to central Paris in under forty minutes.
        The airport itself—designed by Paul Andreu and opened in 1974—is noted for
        its striking brutalist Terminal 1, a circular concrete drum with satellite
        pods connected by moving walkways. It handles over seventy million passengers
        a year, making it a true gateway to France.
"""

    let airports =
        [ ("Charles de Gaulle Airport", 90<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createAirport street.Id)

    let hotels =
        [ ("Sheraton Paris Charles de Gaulle", 85<quality>, 280m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    let metroStation =
        ("CDG Airport Station", zone.Id)
        |> (PlaceCreators.createMetro street.Id)

    let street =
        street
        |> World.Street.addPlaces airports
        |> World.Street.addPlaces hotels
        |> World.Street.addPlace metroStation

    street, metroStation

let zone =
    let roissyZone = World.Zone.create Ids.Zone.roissy

    let avenueCharlesDeGaulle, cdgStation = avenueCharlesDeGaulle roissyZone

    let station =
        { Lines = [ Blue ]
          LeavesToStreet = avenueCharlesDeGaulle.Id
          PlaceId = cdgStation.Id }

    roissyZone
    |> World.Zone.addStreet (
        World.Node.create avenueCharlesDeGaulle.Id avenueCharlesDeGaulle
    )
    |> World.Zone.addMetroStation station

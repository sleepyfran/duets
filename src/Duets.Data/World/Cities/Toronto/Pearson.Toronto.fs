module Duets.Data.World.Cities.Toronto.Pearson

open Duets.Data.World.Cities.Toronto
open Duets.Data.World.Cities
open Duets.Entities

let private airportRoad (zone: Zone) =
    let street =
        World.Street.create Ids.Street.airportRoad StreetType.OneWay

    let airports =
        [ ("Toronto Pearson International Airport", 94<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createAirport street.Id)

    let hotels =
        [ ("Sheraton Gateway", 82<quality>, 280m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    let metroStation =
        ("Pearson Airport Station", zone.Id)
        |> PlaceCreators.createMetro street.Id

    let street =
        street
        |> World.Street.addPlaces airports
        |> World.Street.addPlaces hotels
        |> World.Street.addPlace metroStation
        |> World.Street.attachContext
            """
        Airport Road leads to Toronto Pearson International Airport, Canada's
        busiest hub. The wide road is flanked by airport hotels, rental car lots,
        and the elevated guideway of the UP Express. Jets descend overhead toward
        parallel runways while taxis and rideshares jostle for terminal curb space.
        The area has a utilitarian, transit-focused atmosphere with constant movement
        of travelers and luggage carts between terminals and parking garages.
"""

    street, metroStation

let zone =
    let pearsonZone = World.Zone.create Ids.Zone.pearson

    let airportRoad, pearsonMetro = airportRoad pearsonZone

    let pearsonMetroStation =
        { Lines = [ Red ]
          LeavesToStreet = airportRoad.Id
          PlaceId = pearsonMetro.Id }

    pearsonZone
    |> World.Zone.addStreet (
        World.Node.create airportRoad.Id airportRoad
    )
    |> World.Zone.addMetroStation pearsonMetroStation

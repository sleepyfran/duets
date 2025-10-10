module rec Duets.Data.World.Cities.Madrid.Barajas

open Duets.Entities
open Duets.Entities.Calendar
open Duets.Data.World.Cities

let aeropuerto (zone: Zone) =
    let street =
        World.Street.create "Aeropuerto" (StreetType.Split(NorthEast, 1))
        |> World.Street.attachContext
            """
        A modern transportation hub dominated by the iconic Adolfo Suárez Madrid-Barajas Airport.
        The area features wide access roads with clear signage in multiple languages,
        taxi ranks, and bus stops. The architecture is characterized by the airport's
        distinctive undulating roof designed by Richard Rogers and Antonio Lamela.
        Palm trees line some sections, and there's constant movement of travelers with luggage,
        airport shuttles, and service vehicles.
"""

    let carDealers =
        [ ("Autos Barajas",
           71<quality>,
           zone.Id,
           { Dealer =
               (Character.from
                   "Carlos Mendoza"
                   Male
                   (Shorthands.Spring 28<days> 1979<years>))
             PriceRange = CarPriceRange.Budget }) ]
        |> List.map (PlaceCreators.createCarDealer street.Id)

    let airport =
        ("Aeropuerto Adolfo Suárez Madrid-Barajas", 90<quality>, zone.Id)
        |> PlaceCreators.createAirport street.Id

    let metroStation =
        ("Barajas Station", zone.Id) |> PlaceCreators.createMetro street.Id

    let street =
        street
        |> World.Street.addPlaces carDealers
        |> World.Street.addPlace airport
        |> World.Street.addPlace metroStation

    street, metroStation

let zone =
    let barajasZone = World.Zone.create "Barajas"
    let aeropuertoStreet, metroStation = aeropuerto barajasZone

    let station =
        { Lines = [ Blue ]
          LeavesToStreet = aeropuertoStreet.Id
          PlaceId = metroStation.Id }

    barajasZone
    |> World.Zone.addStreet (
        World.Node.create aeropuertoStreet.Id aeropuertoStreet
    )
    |> World.Zone.addMetroStation station

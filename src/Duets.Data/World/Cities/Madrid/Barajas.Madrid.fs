module rec Duets.Data.World.Cities.Madrid.Barajas

open Duets.Entities
open Duets.Data.World.Cities

let aeropuerto (zone: Zone) =
    let street =
        World.Street.create "Aeropuerto" (StreetType.Split(NorthEast, 1))

    let airport =
        ("Aeropuerto Adolfo Suárez Madrid-Barajas", 90<quality>, zone.Id)
        |> PlaceCreators.createAirport street.Id

    let metroStation =
        ("Barajas Station", zone.Id) |> PlaceCreators.createMetro street.Id

    let street =
        street
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
    |> World.Zone.addDescriptor Industrial

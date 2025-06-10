module rec Duets.Data.World.Cities.Prague.Libeň

open Duets.Data.World.Cities
open Duets.Entities

let createZone city =
    let libeňZone = World.Zone.create "Libeň"

    let ocelářská city (zone: Zone) =
        let street = World.Street.create "Ocelářská" (StreetType.Split(East, 2))

        let concertSpaces =
            [ ("O2 Arena",
               18000,
               95<quality>,
               Layouts.concertSpaceLayout2,
               zone.Id)
              ("O2 Universum",
               4500,
               92<quality>,
               Layouts.concertSpaceLayout3,
               zone.Id) ]
            |> List.map (PlaceCreators.createConcertSpace street.Id)

        let metroStation =
            ("Českomoravská Station", zone.Id)
            |> (PlaceCreators.createMetro street.Id)

        let street =
            street
            |> World.Street.addPlaces concertSpaces
            |> World.Street.addPlace metroStation

        street, metroStation

    let ocelářská, metroStation = ocelářská city libeňZone

    let station =
        { Lines = [ Blue ]
          LeavesToStreet = ocelářská.Id
          PlaceId = metroStation.Id }

    libeňZone
    |> World.Zone.addStreet (World.Node.create ocelářská.Id ocelářská)
    |> World.Zone.addMetroStation station
    |> World.Zone.addDescriptor EntertainmentHeart

module rec Duets.Data.World.Cities.Prague.Vršovice

open Duets.Data.World.Cities
open Duets.Entities

let vršovická (zone: Zone) =
    let street = World.Street.create "Vršovická" (StreetType.Split(East, 1))

    let concertSpaces =
        [ ("Eden Aréna",
           21000,
           90<quality>,
           Layouts.concertSpaceLayout1,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let metroStation =
        ("Strašnická Station", zone.Id) |> PlaceCreators.createMetro street.Id

    street
    |> World.Street.addPlaces concertSpaces
    |> World.Street.addPlace metroStation,
    metroStation

let zone =
    let vršoviceZone = World.Zone.create "Vršovice"
    let vršovická, metroStation = vršovická vršoviceZone

    let zoneMetroStation =
        { Lines = [ Red ]
          LeavesToStreet = vršovická.Id
          PlaceId = metroStation.Id }

    vršoviceZone
    |> World.Zone.addStreet (World.Node.create vršovická.Id vršovická)
    |> World.Zone.addDescriptor Industrial
    |> World.Zone.addMetroStation zoneMetroStation

module rec Duets.Data.World.Cities.NewYork.Harlem

open Duets.Data.World.Cities
open Duets.Entities
open Duets.Entities.Calendar

let private street125th city (zone: Zone) =
    let street = World.Street.create "125th Street" (StreetType.Split(East, 4))

    let concertSpaces =
        [ ("Apollo Theater",
           1506,
           80<quality>,
           Layouts.concertSpaceLayout1,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let gyms =
        [ ("Planet Fitness Harlem", 89<quality>, zone.Id)
          ("The Edge Gym", 88<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createGym city street.Id)

    let rehearsalSpaces =
        [ ("Harlem Harmony", 86<quality>, 110m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createRehearsalSpace street.Id)

    let studios =
        [ ("Harlem Harmony Studios",
           86<quality>,
           220m<dd>,
           (Character.from
               "Elisa Miller"
               Female
               (Shorthands.Spring 1<days> 1990<years>)),
           zone.Id) ]
        |> List.map (PlaceCreators.createStudio street.Id)

    let radioStudios =
        [ ("WBGO 88.3 FM", 90<quality>, "Jazz", zone.Id) ]
        |> List.map (PlaceCreators.createRadioStudio city)

    let metroStation =
        ("125th Street Station", zone.Id) |> PlaceCreators.createMetro street.Id

    let street =
        street
        |> World.Street.addPlaces concertSpaces
        |> World.Street.addPlaces gyms
        |> World.Street.addPlaces rehearsalSpaces
        |> World.Street.addPlaces studios
        |> World.Street.addPlaces radioStudios
        |> World.Street.addPlace metroStation

    street, metroStation

let createZone city =
    let harlemZone = World.Zone.create "Harlem"

    let street125th, metroStation = street125th city harlemZone

    let metroStation =
        { Lines = [ Red ]
          LeavesToStreet = street125th.Id
          PlaceId = metroStation.Id }

    harlemZone
    |> World.Zone.addStreet (World.Node.create street125th.Id street125th)
    |> World.Zone.addDescriptor Cultural
    |> World.Zone.addDescriptor Historic
    |> World.Zone.addMetroStation metroStation

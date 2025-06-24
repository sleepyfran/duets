module rec Duets.Data.World.Cities.Sydney.NorthSydney

open Duets.Data.World.Cities
open Duets.Entities
open Duets.Entities.Calendar

let private millerStreet (zone: Zone) =
    let street =
        World.Street.create "Miller Street" (StreetType.Split(North, 2))

    let hotels =
        [ ("Meriton Suites North Sydney", 88<quality>, 650m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    let restaurants =
        [ ("Rengaya", 90<quality>, Japanese, zone.Id)
          ("Greenwood Hotel", 87<quality>, French, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let cafes =
        [ ("Café Lost & Found", 85<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    let bars =
        [ ("The Rag & Famish Hotel", 86<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let bookstores =
        [ ("Constant Reader", 86<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBookstore street.Id)

    let metroStation =
        ("Miller Street Metro", zone.Id)
        |> (PlaceCreators.createMetro street.Id)

    let street =
        street
        |> World.Street.addPlaces hotels
        |> World.Street.addPlaces restaurants
        |> World.Street.addPlaces cafes
        |> World.Street.addPlaces bars
        |> World.Street.addPlaces bookstores
        |> World.Street.addPlace metroStation

    street, metroStation

let private pacificHighway (zone: Zone) =
    let street = World.Street.create "Pacific Highway" StreetType.OneWay

    let studios =
        [ ("North Sydney Studios",
           87<quality>,
           350m<dd>,
           (Character.from
               "Rick Grossman"
               Male
               (Shorthands.Spring 1<days> 1959<years>)),
           zone.Id) ]
        |> List.map (PlaceCreators.createStudio street.Id)

    let metroStation =
        ("Pacific Highway Metro", zone.Id)
        |> (PlaceCreators.createMetro street.Id)

    let street =
        street
        |> World.Street.addPlaces studios
        |> World.Street.addPlace metroStation

    street

let zone =
    let northSydneyZone = World.Zone.create "North Sydney"
    let millerStreet, millerStreetMetro = millerStreet northSydneyZone
    let pacificHighway = pacificHighway northSydneyZone

    let millerStreetMetroStation =
        { Lines = [ Red ]
          LeavesToStreet = millerStreet.Id
          PlaceId = millerStreetMetro.Id }

    northSydneyZone
    |> World.Zone.addStreet (World.Node.create millerStreet.Id millerStreet)
    |> World.Zone.addStreet (World.Node.create pacificHighway.Id pacificHighway)
    |> World.Zone.connectStreets millerStreet.Id pacificHighway.Id East
    |> World.Zone.addDescriptor BusinessDistrict
    |> World.Zone.addDescriptor Creative
    |> World.Zone.addMetroStation millerStreetMetroStation

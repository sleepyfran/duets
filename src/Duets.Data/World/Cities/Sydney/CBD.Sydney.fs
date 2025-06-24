module rec Duets.Data.World.Cities.Sydney.CBD

open Duets.Data.World.Cities
open Duets.Entities

let private georgeStreet (zone: Zone) city =
    let street =
        World.Street.create "George Street" (StreetType.Split(North, 3))

    let home = PlaceCreators.createHome street.Id zone.Id

    let hotels =
        [ ("Hilton Sydney", 93<quality>, 800m<dd>, zone.Id)
          ("QT Sydney", 91<quality>, 750m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    let restaurants =
        [ ("Mr. Wong", 94<quality>, Japanese, zone.Id)
          ("The Grounds of the City", 92<quality>, French, zone.Id)
          ("Bar Luca", 90<quality>, American, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let cafes =
        [ ("Mecca Coffee", 89<quality>, zone.Id)
          ("Workshop Espresso", 87<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    let bookstores =
        [ ("Kinokuniya", 92<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBookstore street.Id)

    let metroStation =
        ("Town Hall Station", zone.Id) |> (PlaceCreators.createMetro street.Id)

    let hospital =
        ("Sydney Hospital", 88<quality>, zone.Id)
        |> (PlaceCreators.createHospital street.Id)

    let gym =
        PlaceCreators.createGym
            city
            street.Id
            ("Fitness First George St", 85<quality>, zone.Id)

    let street =
        street
        |> World.Street.addPlaces hotels
        |> World.Street.addPlaces restaurants
        |> World.Street.addPlaces cafes
        |> World.Street.addPlaces bookstores
        |> World.Street.addPlace metroStation
        |> World.Street.addPlace hospital
        |> World.Street.addPlace gym
        |> World.Street.addPlace home

    street, metroStation

let private pittStreet (zone: Zone) =
    let street = World.Street.create "Pitt Street" (StreetType.Split(South, 2))

    let hotels =
        [ ("Swissôtel Sydney", 90<quality>, 700m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    let restaurants =
        [ ("Fratelli Fresh", 91<quality>, Italian, zone.Id)
          ("Din Tai Fung", 93<quality>, Japanese, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let bars =
        [ ("The Baxter Inn", 92<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let metroStation =
        ("Pitt Street Metro", zone.Id) |> (PlaceCreators.createMetro street.Id)

    let street =
        street
        |> World.Street.addPlaces hotels
        |> World.Street.addPlaces restaurants
        |> World.Street.addPlaces bars
        |> World.Street.addPlace metroStation

    street

let private martinPlace (zone: Zone) =
    let street = World.Street.create "Martin Place" StreetType.OneWay

    let concertSpaces =
        [ ("City Recital Hall",
           1200,
           93<quality>,
           Layouts.concertSpaceLayout2,
           zone.Id)
          ("Foundry 616", 200, 86<quality>, Layouts.concertSpaceLayout1, zone.Id)
          ("Venue 505", 200, 85<quality>, Layouts.concertSpaceLayout1, zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let metroStation =
        ("Martin Place Metro", zone.Id) |> (PlaceCreators.createMetro street.Id)

    let street =
        street
        |> World.Street.addPlaces concertSpaces
        |> World.Street.addPlace metroStation

    street

let createZone city =
    let cbdZone = World.Zone.create "CBD"
    let georgeStreet, metroStation = georgeStreet cbdZone city
    let pittStreet = pittStreet cbdZone
    let martinPlace = martinPlace cbdZone

    let metroStation =
        { Lines = [ Blue; Red ]
          LeavesToStreet = georgeStreet.Id
          PlaceId = metroStation.Id }

    cbdZone
    |> World.Zone.addStreet (World.Node.create georgeStreet.Id georgeStreet)
    |> World.Zone.addStreet (World.Node.create pittStreet.Id pittStreet)
    |> World.Zone.addStreet (World.Node.create martinPlace.Id martinPlace)
    |> World.Zone.addDescriptor BusinessDistrict
    |> World.Zone.addDescriptor Historic
    |> World.Zone.addMetroStation metroStation

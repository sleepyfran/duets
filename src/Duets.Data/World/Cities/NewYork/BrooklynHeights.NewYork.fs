module rec Duets.Data.World.Cities.NewYork.BrooklynHeights

open Duets.Data.World.Cities
open Duets.Entities
open Duets.Entities.Calendar

let private montagueStreet (zone: Zone) =
    let street =
        World.Street.create "Montague Street" (StreetType.Split(East, 3))

    let bookstores =
        [ ("Books Are Magic", 92<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBookstore street.Id)

    let cafes =
        [ ("Cup of Joy", 90<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    let restaurants =
        [ ("Juliana's Pizza", 88<quality>, Italian, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let metroStation =
        ("Brooklyn Heights Station", zone.Id)
        |> PlaceCreators.createMetro street.Id

    let street =
        street
        |> World.Street.addPlaces bookstores
        |> World.Street.addPlaces cafes
        |> World.Street.addPlaces restaurants
        |> World.Street.addPlace metroStation

    street, metroStation

let private flatbushAvenue city (zone: Zone) =
    let street =
        World.Street.create "Flatbush Avenue" (StreetType.Split(North, 4))

    let bars =
        [ ("Brooklyn Brewery", 92<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let concertSpaces =
        [ ("Barclays Center",
           19000,
           88<quality>,
           Layouts.concertSpaceLayout2,
           zone.Id)
          ("Rough Trade NYC",
           250,
           80<quality>,
           Layouts.concertSpaceLayout4,
           zone.Id)
          ("Kings Theatre",
           3000,
           87<quality>,
           Layouts.concertSpaceLayout4,
           zone.Id)
          ("The Brooklyn Academy of Music",
           2100,
           88<quality>,
           Layouts.concertSpaceLayout4,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let gyms =
        [ ("Brooklyn Boulders Gowanus", 88<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createGym city street.Id)

    let rehearsalSpaces =
        [ ("Sound Space", 90<quality>, 150m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createRehearsalSpace street.Id)

    let restaurants =
        [ ("Peter Luger Steak House", 85<quality>, American, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let studios =
        [ ("Brooklyn Studio",
           90<quality>,
           300m<dd>,
           (Character.from
               "Eva Johnson"
               Female
               (Shorthands.Spring 15<days> 1980<years>)),
           zone.Id) ]
        |> List.map (PlaceCreators.createStudio street.Id)

    street
    |> World.Street.addPlaces bars
    |> World.Street.addPlaces concertSpaces
    |> World.Street.addPlaces gyms
    |> World.Street.addPlaces rehearsalSpaces
    |> World.Street.addPlaces restaurants
    |> World.Street.addPlaces studios

let createZone city =
    let brooklynHeightsZone = World.Zone.create "Brooklyn Heights"

    let montagueStreet, metroStation = montagueStreet brooklynHeightsZone
    let flatbushAvenue = flatbushAvenue city brooklynHeightsZone

    let metroStation =
        { Lines = [ Blue ]
          LeavesToStreet = montagueStreet.Id
          PlaceId = metroStation.Id }

    brooklynHeightsZone
    |> World.Zone.addStreet (World.Node.create montagueStreet.Id montagueStreet)
    |> World.Zone.addStreet (World.Node.create flatbushAvenue.Id flatbushAvenue)
    |> World.Zone.connectStreets montagueStreet.Id flatbushAvenue.Id South
    |> World.Zone.addDescriptor Historic
    |> World.Zone.addDescriptor Coastal
    |> World.Zone.addMetroStation metroStation

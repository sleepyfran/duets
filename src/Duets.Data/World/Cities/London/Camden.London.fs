module rec Duets.Data.World.Cities.London.Camden

open Duets.Data.World.Cities
open Duets.Entities

let private camdenHighStreet (zone: Zone) =
    let street =
        World.Street.create "Camden High Street" (StreetType.Split(North, 2))

    let bars =
        [ ("The World's End", 80<quality>, zone.Id)
          ("Dublin Castle", 78<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let concertSpaces =
        [ ("Roundhouse", 1700, 88<quality>, Layouts.concertSpaceLayout2, zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let rehearsalSpaces =
        [ ("Camden Practice Rooms", 75<quality>, 160m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createRehearsalSpace street.Id)

    let metroStation =
        ("Camden Town Station", zone.Id)
        |> (PlaceCreators.createMetro street.Id)

    let street =
        street
        |> World.Street.addPlaces bars
        |> World.Street.addPlaces concertSpaces
        |> World.Street.addPlaces rehearsalSpaces
        |> World.Street.addPlace metroStation

    street, metroStation

let private chalkFarm (zone: Zone) =
    let street = World.Street.create "Chalk Farm" (StreetType.Split(West, 2))

    let cafes =
        [ ("The Coffee Jar", 75<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    let bookstores =
        [ ("Owl Bookshop", 80<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBookstore street.Id)

    street |> World.Street.addPlaces cafes |> World.Street.addPlaces bookstores

let private regentsPark (zone: Zone) =
    let street = World.Street.create "Regent's Park" StreetType.OneWay

    let concertSpaces =
        [ ("Open Air Theatre",
           1250,
           85<quality>,
           Layouts.concertSpaceLayout1,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let cafes =
        [ ("Regent's Park Cafe", 70<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    street
    |> World.Street.addPlaces concertSpaces
    |> World.Street.addPlaces cafes

let zone =
    let zone = World.Zone.create "Camden"
    let camdenHighStreet, metroStation = camdenHighStreet zone
    let chalkFarm = chalkFarm zone
    let regentsPark = regentsPark zone

    let metroStation =
        { Lines = [ Red; Blue ]
          LeavesToStreet = camdenHighStreet.Id
          PlaceId = metroStation.Id }

    zone
    |> World.Zone.addStreet (
        World.Node.create camdenHighStreet.Id camdenHighStreet
    )
    |> World.Zone.addStreet (World.Node.create chalkFarm.Id chalkFarm)
    |> World.Zone.addStreet (World.Node.create regentsPark.Id regentsPark)
    |> World.Zone.connectStreets camdenHighStreet.Id chalkFarm.Id West
    |> World.Zone.connectStreets camdenHighStreet.Id regentsPark.Id North
    |> World.Zone.addDescriptor Creative
    |> World.Zone.addDescriptor Cultural
    |> World.Zone.addMetroStation metroStation

module Duets.Data.World.Cities.LosAngeles.DowntownLA

open Duets.Data.World.Cities
open Duets.Entities

let figueroaStreet (zone: Zone) =
    let street =
        World.Street.create
            Ids.Street.figueroaStreet
            (StreetType.Split(North, 2))

    let concertSpaces =
        [ ("Crypto.com Arena",
           20000,
           98<quality>,
           Layouts.concertSpaceLayout2,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    street |> World.Street.addPlaces concertSpaces

let olympicBoulevard (zone: Zone) =
    let street =
        World.Street.create
            Ids.Street.olympicBoulevard
            (StreetType.Split(East, 2))

    let concertSpaces =
        [ ("The Novo", 2400, 90<quality>, Layouts.concertSpaceLayout4, zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    street |> World.Street.addPlaces concertSpaces

let chickHearnCourt (zone: Zone) =
    let street =
        World.Street.create
            Ids.Street.chickHearnCourt
            (StreetType.Split(East, 2))

    let concertSpaces =
        [ ("Peacock Theater",
           7100,
           94<quality>,
           Layouts.concertSpaceLayout1,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    street |> World.Street.addPlaces concertSpaces

let hillStreet (zone: Zone) =
    let street =
        World.Street.create Ids.Street.hillStreet (StreetType.Split(North, 2))

    let concertSpaces =
        [ ("The Belasco",
           1500,
           88<quality>,
           Layouts.concertSpaceLayout3,
           zone.Id)
          ("The Mayan", 1491, 87<quality>, Layouts.concertSpaceLayout3, zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    street |> World.Street.addPlaces concertSpaces

let broadway (zone: Zone) =
    let street =
        World.Street.create Ids.Street.broadway (StreetType.Split(North, 2))

    let concertSpaces =
        [ ("Orpheum Theatre",
           2000,
           91<quality>,
           Layouts.concertSpaceLayout4,
           zone.Id)
          ("The Theatre at Ace Hotel",
           1600,
           90<quality>,
           Layouts.concertSpaceLayout4,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    street |> World.Street.addPlaces concertSpaces

let mainStreet (zone: Zone) =
    let street =
        World.Street.create Ids.Street.mainStreet (StreetType.Split(North, 2))

    let concertSpaces =
        [ ("The Smell", 100, 75<quality>, Layouts.concertSpaceLayout1, zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    street |> World.Street.addPlaces concertSpaces

let boylstonStreet (zone: Zone) =
    let street =
        World.Street.create
            Ids.Street.boylstonStreet
            (StreetType.Split(North, 2))

    let concertSpaces =
        [ ("The Bellwether",
           1600,
           89<quality>,
           Layouts.concertSpaceLayout4,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    street |> World.Street.addPlaces concertSpaces

let seventhStreet (zone: Zone) =
    let street =
        World.Street.create Ids.Street.seventhStreet (StreetType.Split(East, 2))

    let concertSpaces =
        [ ("Teragram Ballroom",
           600,
           86<quality>,
           Layouts.concertSpaceLayout2,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let metroStation =
        ("7th St/Metro Center", zone.Id)
        |> (PlaceCreators.createMetro street.Id)

    let street =
        street
        |> World.Street.addPlaces concertSpaces
        |> World.Street.addPlace metroStation

    street, metroStation

let firstStreet (zone: Zone) =
    let street =
        World.Street.create Ids.Street.firstStreet (StreetType.Split(East, 2))

    let concertSpaces =
        [ ("Moroccan Lounge",
           275,
           84<quality>,
           Layouts.concertSpaceLayout1,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    street |> World.Street.addPlaces concertSpaces

let hewittStreet (zone: Zone) =
    let street =
        World.Street.create Ids.Street.hewittStreet (StreetType.Split(North, 2))

    let concertSpaces =
        [ ("Resident", 200, 83<quality>, Layouts.concertSpaceLayout1, zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    street |> World.Street.addPlaces concertSpaces

let zone =
    let downtownLAZone = World.Zone.create Ids.Zone.downtownLA

    let figueroaStreet = figueroaStreet downtownLAZone
    let olympicBoulevard = olympicBoulevard downtownLAZone
    let chickHearnCourt = chickHearnCourt downtownLAZone
    let hillStreet = hillStreet downtownLAZone
    let broadway = broadway downtownLAZone
    let mainStreet = mainStreet downtownLAZone
    let boylstonStreet = boylstonStreet downtownLAZone
    let seventhStreet, seventhStreetStation = seventhStreet downtownLAZone
    let firstStreet = firstStreet downtownLAZone
    let hewittStreet = hewittStreet downtownLAZone

    let station =
        { Lines = [ Red ]
          LeavesToStreet = seventhStreet.Id
          PlaceId = seventhStreetStation.Id }

    downtownLAZone
    |> World.Zone.addStreet (World.Node.create figueroaStreet.Id figueroaStreet)
    |> World.Zone.addStreet (
        World.Node.create olympicBoulevard.Id olympicBoulevard
    )
    |> World.Zone.addStreet (
        World.Node.create chickHearnCourt.Id chickHearnCourt
    )
    |> World.Zone.addStreet (World.Node.create hillStreet.Id hillStreet)
    |> World.Zone.addStreet (World.Node.create broadway.Id broadway)
    |> World.Zone.addStreet (World.Node.create mainStreet.Id mainStreet)
    |> World.Zone.addStreet (World.Node.create boylstonStreet.Id boylstonStreet)
    |> World.Zone.addStreet (World.Node.create seventhStreet.Id seventhStreet)
    |> World.Zone.addStreet (World.Node.create firstStreet.Id firstStreet)
    |> World.Zone.addStreet (World.Node.create hewittStreet.Id hewittStreet)
    |> World.Zone.connectStreets figueroaStreet.Id olympicBoulevard.Id East
    |> World.Zone.connectStreets olympicBoulevard.Id chickHearnCourt.Id East
    |> World.Zone.connectStreets chickHearnCourt.Id hillStreet.Id East
    |> World.Zone.connectStreets hillStreet.Id broadway.Id East
    |> World.Zone.connectStreets broadway.Id mainStreet.Id East
    |> World.Zone.connectStreets mainStreet.Id boylstonStreet.Id East
    |> World.Zone.connectStreets boylstonStreet.Id seventhStreet.Id East
    |> World.Zone.connectStreets seventhStreet.Id firstStreet.Id East
    |> World.Zone.connectStreets firstStreet.Id hewittStreet.Id East
    |> World.Zone.addMetroStation station

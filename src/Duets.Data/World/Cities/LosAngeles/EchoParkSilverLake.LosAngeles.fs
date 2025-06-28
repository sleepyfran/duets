module Duets.Data.World.Cities.LosAngeles.EchoParkSilverLake

open Duets.Data.World.Cities.LosAngeles
open Duets.Data.World.Cities
open Duets.Entities

let sunsetBoulevard (zone: Zone) =
    let street =
        World.Street.create
            Ids.Street.sunsetBoulevardEchoPark
            (StreetType.Split(East, 2))

    let concertSpaces =
        [ ("The Echo", 350, 85<quality>, Layouts.concertSpaceLayout2, zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    street |> World.Street.addPlaces concertSpaces

let glendaleBoulevard (zone: Zone) =
    let street =
        World.Street.create
            Ids.Street.glendaleBoulevard
            (StreetType.Split(North, 2))

    let concertSpaces =
        [ ("Echoplex", 780, 87<quality>, Layouts.concertSpaceLayout3, zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    street |> World.Street.addPlaces concertSpaces

let nFigueroaStreet (zone: Zone) =
    let street =
        World.Street.create
            Ids.Street.nFigueroaStreet
            (StreetType.Split(North, 2))

    let concertSpaces =
        [ ("Lodge Room", 500, 86<quality>, Layouts.concertSpaceLayout3, zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let metroStation =
        ("Figueroa/Sunset Station", zone.Id)
        |> (PlaceCreators.createMetro street.Id)

    let street =
        street
        |> World.Street.addPlaces concertSpaces
        |> World.Street.addPlace metroStation

    street, metroStation

let silverLakeBoulevard (zone: Zone) =
    let street =
        World.Street.create
            Ids.Street.silverLakeBoulevard
            (StreetType.Split(North, 2))

    let concertSpaces =
        [ ("The Satellite",
           130,
           84<quality>,
           Layouts.concertSpaceLayout1,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    street |> World.Street.addPlaces concertSpaces

let zone =
    let echoParkSilverLakeZone = World.Zone.create Ids.Zone.echoParkSilverLake

    let sunsetBoulevard = sunsetBoulevard echoParkSilverLakeZone
    let glendaleBoulevard = glendaleBoulevard echoParkSilverLakeZone

    let nFigueroaStreet, figueroaStation =
        nFigueroaStreet echoParkSilverLakeZone

    let silverLakeBoulevard = silverLakeBoulevard echoParkSilverLakeZone

    let station =
        { Lines = [ Blue ]
          LeavesToStreet = nFigueroaStreet.Id
          PlaceId = figueroaStation.Id }

    echoParkSilverLakeZone
    |> World.Zone.addStreet (
        World.Node.create sunsetBoulevard.Id sunsetBoulevard
    )
    |> World.Zone.addStreet (
        World.Node.create glendaleBoulevard.Id glendaleBoulevard
    )
    |> World.Zone.addStreet (
        World.Node.create nFigueroaStreet.Id nFigueroaStreet
    )
    |> World.Zone.addStreet (
        World.Node.create silverLakeBoulevard.Id silverLakeBoulevard
    )
    |> World.Zone.connectStreets sunsetBoulevard.Id glendaleBoulevard.Id North
    |> World.Zone.connectStreets glendaleBoulevard.Id nFigueroaStreet.Id East
    |> World.Zone.connectStreets nFigueroaStreet.Id silverLakeBoulevard.Id North
    |> World.Zone.addMetroStation station

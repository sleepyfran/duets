module Duets.Data.World.Cities.LosAngeles.Westside

open Duets.Data.World.Cities.LosAngeles
open Duets.Data.World.Cities
open Duets.Entities

let sunsetBoulevardWestside (zone: Zone) =
    let street =
        World.Street.create
            Ids.Street.sunsetBoulevardWestside
            (StreetType.Split(East, 2))

    let concertSpaces =
        [ ("The Roxy Theatre",
           500,
           87<quality>,
           Layouts.concertSpaceLayout3,
           zone.Id)
          ("The Viper Room",
           250,
           85<quality>,
           Layouts.concertSpaceLayout1,
           zone.Id)
          ("Whisky a Go Go",
           500,
           88<quality>,
           Layouts.concertSpaceLayout3,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    street |> World.Street.addPlaces concertSpaces

let wilshireBoulevard (zone: Zone) =
    let street =
        World.Street.create
            Ids.Street.wilshireBoulevardWestside
            (StreetType.Split(East, 2))

    let concertSpaces =
        [ ("The Saban", 1897, 91<quality>, Layouts.concertSpaceLayout4, zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let metroStation =
        ("Wilshire/Western Station", zone.Id)
        |> (PlaceCreators.createMetro street.Id)

    let street =
        street
        |> World.Street.addPlaces concertSpaces
        |> World.Street.addPlace metroStation

    street, metroStation

let santaMonicaBoulevardWestside (zone: Zone) =
    let street =
        World.Street.create
            Ids.Street.santaMonicaBoulevardWestside
            (StreetType.Split(East, 2))

    let concertSpaces =
        [ ("Troubadour", 500, 86<quality>, Layouts.concertSpaceLayout3, zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    street |> World.Street.addPlaces concertSpaces

let zone =
    let westsideZone = World.Zone.create Ids.Zone.westside

    let sunsetBoulevard = sunsetBoulevardWestside westsideZone
    let wilshireBoulevard, wilshireStation = wilshireBoulevard westsideZone
    let santaMonicaBoulevard = santaMonicaBoulevardWestside westsideZone

    let station =
        { Lines = [ Red ]
          LeavesToStreet = wilshireBoulevard.Id
          PlaceId = wilshireStation.Id }

    westsideZone
    |> World.Zone.addStreet (
        World.Node.create sunsetBoulevard.Id sunsetBoulevard
    )
    |> World.Zone.addStreet (
        World.Node.create wilshireBoulevard.Id wilshireBoulevard
    )
    |> World.Zone.addStreet (
        World.Node.create santaMonicaBoulevard.Id santaMonicaBoulevard
    )
    |> World.Zone.connectStreets sunsetBoulevard.Id wilshireBoulevard.Id North
    |> World.Zone.connectStreets
        wilshireBoulevard.Id
        santaMonicaBoulevard.Id
        North
    |> World.Zone.addMetroStation station

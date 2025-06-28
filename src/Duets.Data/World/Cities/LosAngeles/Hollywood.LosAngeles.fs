module Duets.Data.World.Cities.LosAngeles.Hollywood

open Duets.Data.World.Cities.LosAngeles
open Duets.Data.World.Cities
open Duets.Entities

let hollywoodBoulevard (zone: Zone) =
    let street =
        World.Street.create
            Ids.Street.hollywoodBoulevard
            (StreetType.Split(East, 2))

    let concertSpaces =
        [ ("The Fonda Theatre",
           1200,
           88<quality>,
           Layouts.concertSpaceLayout3,
           zone.Id)
          ("Dolby Theatre",
           3400,
           92<quality>,
           Layouts.concertSpaceLayout4,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    street |> World.Street.addPlaces concertSpaces

let highlandAvenue (zone: Zone) =
    let street =
        World.Street.create
            Ids.Street.highlandAvenue
            (StreetType.Split(North, 2))

    let concertSpaces =
        [ ("Hollywood Bowl",
           17500,
           95<quality>,
           Layouts.concertSpaceLayout3,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let metroStation =
        ("Hollywood/Highland Station", zone.Id)
        |> (PlaceCreators.createMetro street.Id)

    let street =
        street
        |> World.Street.addPlaces concertSpaces
        |> World.Street.addPlace metroStation

    street, metroStation

let sunsetBoulevard (zone: Zone) =
    let street =
        World.Street.create
            Ids.Street.sunsetBoulevard
            (StreetType.Split(East, 2))

    let concertSpaces =
        [ ("Hollywood Palladium",
           3700,
           89<quality>,
           Layouts.concertSpaceLayout4,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    street |> World.Street.addPlaces concertSpaces

let vineStreet (zone: Zone) =
    let street =
        World.Street.create Ids.Street.vineStreet (StreetType.Split(North, 2))

    let concertSpaces =
        [ ("Avalon Hollywood",
           1500,
           87<quality>,
           Layouts.concertSpaceLayout3,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    street |> World.Street.addPlaces concertSpaces

let cahuengaBoulevard (zone: Zone) =
    let street =
        World.Street.create
            Ids.Street.cahuengaBoulevard
            (StreetType.Split(North, 2))

    let concertSpaces =
        [ ("Hotel Cafe", 215, 85<quality>, Layouts.concertSpaceLayout1, zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    street |> World.Street.addPlaces concertSpaces

let santaMonicaBoulevard (zone: Zone) =
    let street =
        World.Street.create
            Ids.Street.santaMonicaBoulevard
            (StreetType.Split(East, 2))

    let concertSpaces =
        [ ("Hollywood Forever Cemetery",
           3000,
           86<quality>,
           Layouts.concertSpaceLayout4,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    street |> World.Street.addPlaces concertSpaces

let zone =
    let hollywoodZone = World.Zone.create Ids.Zone.hollywood

    let hollywoodBoulevard = hollywoodBoulevard hollywoodZone
    let highlandAvenue, highlandAvenueStation = highlandAvenue hollywoodZone
    let sunsetBoulevard = sunsetBoulevard hollywoodZone
    let vineStreet = vineStreet hollywoodZone
    let cahuengaBoulevard = cahuengaBoulevard hollywoodZone
    let santaMonicaBoulevard = santaMonicaBoulevard hollywoodZone

    let station =
        { Lines = [ Red ]
          LeavesToStreet = highlandAvenue.Id
          PlaceId = highlandAvenueStation.Id }

    hollywoodZone
    |> World.Zone.addStreet (
        World.Node.create hollywoodBoulevard.Id hollywoodBoulevard
    )
    |> World.Zone.addStreet (World.Node.create highlandAvenue.Id highlandAvenue)
    |> World.Zone.addStreet (
        World.Node.create sunsetBoulevard.Id sunsetBoulevard
    )
    |> World.Zone.addStreet (World.Node.create vineStreet.Id vineStreet)
    |> World.Zone.addStreet (
        World.Node.create cahuengaBoulevard.Id cahuengaBoulevard
    )
    |> World.Zone.addStreet (
        World.Node.create santaMonicaBoulevard.Id santaMonicaBoulevard
    )
    |> World.Zone.connectStreets cahuengaBoulevard.Id hollywoodBoulevard.Id East
    |> World.Zone.connectStreets hollywoodBoulevard.Id vineStreet.Id East
    |> World.Zone.connectStreets vineStreet.Id highlandAvenue.Id East
    |> World.Zone.connectStreets sunsetBoulevard.Id vineStreet.Id North
    |> World.Zone.connectStreets vineStreet.Id santaMonicaBoulevard.Id North
    |> World.Zone.addMetroStation station

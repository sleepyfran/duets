module rec Duets.Data.World.Cities.Prague.Smíchov

open Duets.Data.World.Cities.Prague
open Duets.Data.World.Cities
open Duets.Entities
open Duets.Entities.Calendar

let plzeňská (zone: Zone) =
    let street = World.Street.create "Plzeňská" (StreetType.Split(East, 2))

    let bars =
        [ ("The Pub Praha 5", 86<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let restaurants =
        [ ("El Sabor Mexicano", 89<quality>, Mexican, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let metroStation =
        ("Anděl Station", zone.Id) |> (PlaceCreators.createMetro street.Id)

    let street =
        street
        |> World.Street.addPlaces bars
        |> World.Street.addPlaces restaurants
        |> World.Street.addPlace metroStation

    street, metroStation

let štefánikova city (zone: Zone) =
    let street = World.Street.create "Štefánikova" StreetType.OneWay

    let bars =
        [ ("BackDoors Bar", 84<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let restaurants =
        [ ("Modrý zub", 87<quality>, Vietnamese, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let gyms =
        [ ("John Reed Fitness", 90<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createGym city street.Id)

    let recordingStudios =
        [ ("Smíchov Sound Studio",
           87<quality>,
           220m<dd>,
           (Character.from
               "Tomáš Novák"
               Male
               (Shorthands.Winter 5<days> 1980<years>)),
           zone.Id) ]
        |> List.map (PlaceCreators.createStudio street.Id)

    street
    |> World.Street.addPlaces bars
    |> World.Street.addPlaces restaurants
    |> World.Street.addPlaces gyms
    |> World.Street.addPlaces recordingStudios

let keSklárně (zone: Zone) =
    let street = World.Street.create "Ke Sklárně" StreetType.OneWay

    let concertSpaces =
        [ ("MeetFactory", 500, 88<quality>, Layouts.concertSpaceLayout2, zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    street |> World.Street.addPlaces concertSpaces

let janáčkovoNábřeží (zone: Zone) =
    let street =
        World.Street.create Ids.Street.janáčkovoNábřeží StreetType.OneWay

    let concertSpaces =
        [ ("Jazz Dock", 150, 95<quality>, Layouts.concertSpaceLayout2, zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    street |> World.Street.addPlaces concertSpaces

let zborovská (zone: Zone) =
    let street = World.Street.create "Zborovská" StreetType.OneWay

    let concertSpaces =
        [ ("Futurum Music Bar",
           650,
           89<quality>,
           Layouts.concertSpaceLayout4,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    street |> World.Street.addPlaces concertSpaces

let createZone city =
    let smíchovZone = World.Zone.create Ids.Zone.smíchov

    let plzeňská, metroStation = plzeňská smíchovZone
    let štefánikova = štefánikova city smíchovZone
    let zborovská = zborovská smíchovZone
    let janáčkovoNábřeží = janáčkovoNábřeží smíchovZone
    let keSklárně = keSklárně smíchovZone

    let station =
        { Lines = [ Blue ]
          LeavesToStreet = plzeňská.Id
          PlaceId = metroStation.Id }

    smíchovZone
    |> World.Zone.addStreet (World.Node.create plzeňská.Id plzeňská)
    |> World.Zone.addStreet (World.Node.create štefánikova.Id štefánikova)
    |> World.Zone.addStreet (World.Node.create zborovská.Id zborovská)
    |> World.Zone.addStreet (
        World.Node.create janáčkovoNábřeží.Id janáčkovoNábřeží
    )
    |> World.Zone.addStreet (World.Node.create keSklárně.Id keSklárně)
    |> World.Zone.connectStreets plzeňská.Id štefánikova.Id North
    |> World.Zone.connectStreets plzeňská.Id zborovská.Id East
    |> World.Zone.connectStreets plzeňská.Id janáčkovoNábřeží.Id South
    |> World.Zone.connectStreets štefánikova.Id keSklárně.Id South
    |> World.Zone.connectStreets štefánikova.Id zborovská.Id East

    |> World.Zone.addMetroStation station

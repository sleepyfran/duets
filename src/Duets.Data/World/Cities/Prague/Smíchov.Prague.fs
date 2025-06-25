module rec Duets.Data.World.Cities.Prague.Smíchov

open Duets.Data.World.Cities
open Duets.Entities
open Duets.Entities.Calendar

let plzeňská (zone: Zone) =
    let street = World.Street.create "Plzeňská" (StreetType.Split(East, 2))

    let concertSpaces =
        [ ("Futurum Music Bar", 650, 89<quality>, Layouts.concertSpaceLayout4, zone.Id)
          ("Jazz Dock", 150, 95<quality>, Layouts.concertSpaceLayout2, zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

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
        |> World.Street.addPlaces concertSpaces
        |> World.Street.addPlaces bars
        |> World.Street.addPlaces restaurants
        |> World.Street.addPlace metroStation

    street, metroStation

let štefánikova city (zone: Zone) =
    let street = World.Street.create "Štefánikova" StreetType.OneWay

    let concertSpaces =
        [ ("MeetFactory", 500, 88<quality>, Layouts.concertSpaceLayout2, zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

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
    |> World.Street.addPlaces concertSpaces
    |> World.Street.addPlaces bars
    |> World.Street.addPlaces restaurants
    |> World.Street.addPlaces gyms
    |> World.Street.addPlaces recordingStudios

let createZone city =
    let smíchovZone = World.Zone.create "Smíchov"

    let plzeňská, metroStation = plzeňská smíchovZone
    let štefánikova = štefánikova city smíchovZone

    let station =
        { Lines = [ Blue ]
          LeavesToStreet = plzeňská.Id
          PlaceId = metroStation.Id }

    smíchovZone
    |> World.Zone.addStreet (World.Node.create plzeňská.Id plzeňská)
    |> World.Zone.addStreet (World.Node.create štefánikova.Id štefánikova)
    |> World.Zone.connectStreets plzeňská.Id štefánikova.Id North
    
    |> World.Zone.addMetroStation station

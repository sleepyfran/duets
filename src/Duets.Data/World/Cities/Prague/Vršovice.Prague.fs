module rec Duets.Data.World.Cities.Prague.Vršovice

open Data.World.Cities.Prague
open Duets.Data.World.Cities
open Duets.Entities

let krymská (zone: Zone) =
    let street =
        World.Street.create Ids.Street.krymská (StreetType.Split(East, 1))

    let concertSpaces =
        [ ("Café v lese", 150, 85<quality>, Layouts.concertSpaceLayout1, zone.Id)
          ("Bad Flash Bar",
           100,
           82<quality>,
           Layouts.concertSpaceLayout2,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let bars =
        [ ("Bad Flash Bar", 88<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let restaurants =
        [ ("Café Sladkovský", 86<quality>, Czech, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    street
    |> World.Street.addPlaces concertSpaces
    |> World.Street.addPlaces bars
    |> World.Street.addPlaces restaurants

let jiříhozPoděbrad (zone: Zone) =
    let street =
        World.Street.create
            Ids.Street.jiříhozPoděbrad
            (StreetType.Split(North, 2))

    let metroStation =
        ("Jiřího z Poděbrad Station", zone.Id)
        |> PlaceCreators.createMetro street.Id

    let street = street |> World.Street.addPlace metroStation

    street, metroStation

let moskevská (zone: Zone) =
    let street = World.Street.create "Moskevská" (StreetType.Split(West, 2))

    let concertSpaces =
        [ ("Waldeska Pub", 80, 78<quality>, Layouts.concertSpaceLayout1, zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let restaurants =
        [ ("U Veverky", 84<quality>, Czech, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    street
    |> World.Street.addPlaces concertSpaces
    |> World.Street.addPlaces restaurants


let zone =
    let vršoviceZone = World.Zone.create "Vršovice"
    let krymská = krymská vršoviceZone
    let moskevská = moskevská vršoviceZone
    let jiříhozPoděbrad, metroStation = jiříhozPoděbrad vršoviceZone

    let zoneMetroStation =
        { Lines = [ Red ]
          LeavesToStreet = jiříhozPoděbrad.Id
          PlaceId = metroStation.Id }

    vršoviceZone
    |> World.Zone.addStreet (World.Node.create krymská.Id krymská)
    |> World.Zone.addStreet (World.Node.create moskevská.Id moskevská)
    |> World.Zone.addStreet (
        World.Node.create jiříhozPoděbrad.Id jiříhozPoděbrad
    )
    |> World.Zone.connectStreets krymská.Id moskevská.Id North
    |> World.Zone.connectStreets krymská.Id jiříhozPoděbrad.Id West

    |> World.Zone.addMetroStation zoneMetroStation

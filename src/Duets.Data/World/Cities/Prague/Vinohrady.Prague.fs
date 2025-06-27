module rec Duets.Data.World.Cities.Prague.Vinohrady

open Data.World.Cities.Prague
open Duets.Data.World.Cities
open Duets.Entities

let francouzská city (zone: Zone) =
    let street = World.Street.create "Francouzská" (StreetType.Split(North, 2))

    let home = PlaceCreators.createHome street.Id zone.Id

    let gyms =
        [ ("Fitness Korunní", 88<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createGym city street.Id)

    let bars =
        [ ("Vinohradský Pivovar", 90<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let restaurants =
        [ ("U Bulínů", 91<quality>, Czech, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let concertSpaces =
        [ ("Retro Music Hall",
           1000,
           80<quality>,
           Layouts.concertSpaceLayout4,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let street =
        street
        |> World.Street.addPlace home
        |> World.Street.addPlaces gyms
        |> World.Street.addPlaces bars
        |> World.Street.addPlaces restaurants

    street

let náměstíMíru (zone: Zone) =
    let street =
        World.Street.create Ids.Street.náměstíMíru (StreetType.Split(East, 3))

    let concertSpaces =
        [ ("Vinohrady Theatre",
           700,
           89<quality>,
           Layouts.concertSpaceLayout4,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let bars =
        [ ("The Down Under", 86<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let restaurants =
        [ ("La Bohème Café", 93<quality>, French, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let metroStation =
        ("Náměstí Míru Station", zone.Id)
        |> (PlaceCreators.createMetro street.Id)

    let street =
        street
        |> World.Street.addPlaces concertSpaces
        |> World.Street.addPlaces bars
        |> World.Street.addPlaces restaurants
        |> World.Street.addPlace metroStation

    street, metroStation

let createZone city =
    let vinohradyZone = World.Zone.create "Vinohrady"

    let francouzská = francouzská city vinohradyZone
    let náměstíMíru, metroStation = náměstíMíru vinohradyZone

    let station =
        { Lines = [ Red ]
          LeavesToStreet = náměstíMíru.Id
          PlaceId = metroStation.Id }

    vinohradyZone
    |> World.Zone.addStreet (World.Node.create francouzská.Id francouzská)
    |> World.Zone.addStreet (World.Node.create náměstíMíru.Id náměstíMíru)
    |> World.Zone.connectStreets francouzská.Id náměstíMíru.Id South

    |> World.Zone.addMetroStation station

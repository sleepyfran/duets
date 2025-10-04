module rec Duets.Data.World.Cities.NewYork.Brooklyn

open Duets.Data.World.Cities
open Duets.Entities

let private lafayetteAtlantic (zone: Zone) =
    let street =
        World.Street.create
            Ids.Street.lafayetteAtlantic
            (StreetType.Split(East, 3))

    let concerts =
        [ ("Barclays Center",
           19000,
           96<quality>,
           Layouts.concertSpaceLayout4,
           zone.Id)
          ("BAM (Brooklyn Academy of Music)",
           2100,
           92<quality>,
           Layouts.concertSpaceLayout3,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let bars =
        [ ("baba cool", 86<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let restaurants =
        [ ("Bacchus", 88<quality>, French, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let metroStation =
        ("Atlantic Avenue-Barclays Center", zone.Id)
        |> PlaceCreators.createMetro street.Id

    let street =
        street
        |> World.Street.addPlaces concerts
        |> World.Street.addPlaces bars
        |> World.Street.addPlaces restaurants
        |> World.Street.addPlace metroStation

    street, metroStation

let private frostStreet (zone: Zone) =
    let street =
        World.Street.create Ids.Street.frostStreet (StreetType.Split(North, 2))

    let home = PlaceCreators.createHome street.Id zone.Id

    // TODO: Add Frost Playground once we support parks
    // TODO: Add Cooper Park once we support parks

    street |> World.Street.addPlace home

let private bedfordAvenue (zone: Zone) =
    let street =
        World.Street.create
            Ids.Street.bedfordAvenue
            (StreetType.Split(North, 2))

    let concerts =
        [ ("The Knitting Factory",
           150,
           87<quality>,
           Layouts.concertSpaceLayout1,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    street |> World.Street.addPlaces concerts

let zone =
    let brooklynZone = World.Zone.create Ids.Zone.brooklyn

    let lafayetteAtlantic, atlanticMetro = lafayetteAtlantic brooklynZone
    let frostStreet = frostStreet brooklynZone
    let bedfordAvenue = bedfordAvenue brooklynZone

    let atlanticMetroStation =
        { Lines = [ Blue ]
          LeavesToStreet = lafayetteAtlantic.Id
          PlaceId = atlanticMetro.Id }

    brooklynZone
    |> World.Zone.addStreet (
        World.Node.create lafayetteAtlantic.Id lafayetteAtlantic
    )
    |> World.Zone.addStreet (World.Node.create frostStreet.Id frostStreet)
    |> World.Zone.addStreet (World.Node.create bedfordAvenue.Id bedfordAvenue)
    |> World.Zone.connectStreets lafayetteAtlantic.Id frostStreet.Id North
    |> World.Zone.connectStreets lafayetteAtlantic.Id bedfordAvenue.Id East
    |> World.Zone.addMetroStation atlanticMetroStation

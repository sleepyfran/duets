module rec Duets.Data.World.Cities.NewYork.MidtownWest

open Duets.Data.World.Cities
open Duets.Entities

let private broadway (zone: Zone) =
    let street =
        World.Street.create Ids.Street.broadway (StreetType.Split(North, 4))

    let home = PlaceCreators.createHome street.Id zone.Id

    let concerts =
        [ ("The Majestic Theatre",
           1500,
           95<quality>,
           Layouts.concertSpaceLayout2,
           zone.Id)
          ("The Gershwin Theatre",
           1900,
           94<quality>,
           Layouts.concertSpaceLayout1,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let bars =
        [ ("TSX Broadway", 88<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let shops =
        [ ("Macy's Herald Square", zone.Id) ]
        |> List.map (PlaceCreators.createMerchandiseWorkshop street.Id)

    let metroStation =
        ("Times Square Station", zone.Id) |> PlaceCreators.createMetro street.Id

    let street =
        street
        |> World.Street.addPlace home
        |> World.Street.addPlaces concerts
        |> World.Street.addPlaces bars
        |> World.Street.addPlaces shops
        |> World.Street.addPlace metroStation

    street, metroStation

let private seventhAvenue (zone: Zone) =
    let street =
        World.Street.create
            Ids.Street.seventhAvenue
            (StreetType.Split(North, 3))

    let concerts =
        [ ("Madison Square Garden",
           20000,
           98<quality>,
           Layouts.concertSpaceLayout4,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let bars =
        [ ("Mustang Harry's", 84<quality>, zone.Id)
          ("Juniper Bar & Grill", 82<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let metroStation =
        ("Penn Station", zone.Id) |> PlaceCreators.createMetro street.Id

    let street =
        street
        |> World.Street.addPlaces concerts
        |> World.Street.addPlaces bars
        |> World.Street.addPlace metroStation

    street, metroStation

let private fiftySeventhStreet (zone: Zone) =
    let street =
        World.Street.create
            Ids.Street.fiftySeventhStreet
            (StreetType.Split(East, 3))

    let concerts =
        [ ("Carnegie Hall",
           2800,
           99<quality>,
           Layouts.concertSpaceLayout3,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let restaurants =
        [ ("The Tea Room", 93<quality>, Czech, zone.Id)
          ("Brooklyn Diner", 85<quality>, American, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let cafes =
        [ ("Carnegie Diner & Cafe", 86<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    street
    |> World.Street.addPlaces concerts
    |> World.Street.addPlaces restaurants
    |> World.Street.addPlaces cafes

let private sixthAvenue (city: City) (zone: Zone) =
    let street =
        World.Street.create Ids.Street.sixthAvenue (StreetType.Split(North, 2))

    let radioStudios =
        [ ("Z100 (WHTZ-FM)", 94<quality>, "Pop", zone.Id)
          ("Q104.3 (WAXQ-FM)", 92<quality>, "Rock", zone.Id) ]
        |> List.map (PlaceCreators.createRadioStudio city)

    street |> World.Street.addPlaces radioStudios

let createZone (city: City) =
    let midtownWestZone = World.Zone.create Ids.Zone.midtownWest

    let broadway, broadwayMetro = broadway midtownWestZone
    let seventhAvenue, pennStation = seventhAvenue midtownWestZone
    let fiftySeventhStreet = fiftySeventhStreet midtownWestZone
    let sixthAvenue = sixthAvenue city midtownWestZone

    let broadwayMetroStation =
        { Lines = [ Blue ]
          LeavesToStreet = broadway.Id
          PlaceId = broadwayMetro.Id }

    let pennStationMetro =
        { Lines = [ Blue ]
          LeavesToStreet = seventhAvenue.Id
          PlaceId = pennStation.Id }

    midtownWestZone
    |> World.Zone.addStreet (World.Node.create broadway.Id broadway)
    |> World.Zone.addStreet (World.Node.create seventhAvenue.Id seventhAvenue)
    |> World.Zone.addStreet (
        World.Node.create fiftySeventhStreet.Id fiftySeventhStreet
    )
    |> World.Zone.addStreet (World.Node.create sixthAvenue.Id sixthAvenue)
    |> World.Zone.connectStreets broadway.Id seventhAvenue.Id West
    |> World.Zone.connectStreets broadway.Id fiftySeventhStreet.Id North
    |> World.Zone.connectStreets broadway.Id sixthAvenue.Id East
    |> World.Zone.addMetroStation broadwayMetroStation
    |> World.Zone.addMetroStation pennStationMetro

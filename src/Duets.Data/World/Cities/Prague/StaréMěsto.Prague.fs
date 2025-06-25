module rec Duets.Data.World.Cities.Prague.StaréMěsto

open Duets.Data.World.Cities
open Duets.Entities

let staroměstskéNáměstí (zone: Zone) =
    let street =
        World.Street.create "Staroměstské náměstí" (StreetType.Split(South, 3))

    let concertSpaces =
        [ ("Roxy Prague", 900, 90<quality>, Layouts.concertSpaceLayout3, zone.Id)
          ("Jazz Republic",
           120,
           88<quality>,
           Layouts.concertSpaceLayout2,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let bars =
        [ ("The Dubliner Irish Bar", 85<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let restaurants =
        [ ("U Prince", 92<quality>, Czech, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let hotels =
        [ ("Grand Hotel Praha", 93<quality>, 250m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    let metroStation =
        ("Staroměstská Station", zone.Id)
        |> (PlaceCreators.createMetro street.Id)

    let street =
        street
        |> World.Street.addPlaces concertSpaces
        |> World.Street.addPlaces bars
        |> World.Street.addPlaces restaurants
        |> World.Street.addPlaces hotels
        |> World.Street.addPlace metroStation

    street, metroStation

let karlova (zone: Zone) =
    let street = World.Street.create "Karlova" (StreetType.Split(West, 2))

    let concertSpaces =
        [ ("Klementinum Mirror Chapel",
           200,
           94<quality>,
           Layouts.concertSpaceLayout4,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let bars =
        [ ("Anonymous Bar", 90<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let restaurants =
        [ ("Terasa U Zlaté studně", 95<quality>, French, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let rehearsalSpaces =
        [ ("Old Town Rehearsal", 85<quality>, 130m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createRehearsalSpace street.Id)

    street
    |> World.Street.addPlaces concertSpaces
    |> World.Street.addPlaces bars
    |> World.Street.addPlaces restaurants
    |> World.Street.addPlaces rehearsalSpaces

let zone =
    let staréMěstoZone = World.Zone.create "Staré Město"

    let staroměstskéNáměstí, metroStation = staroměstskéNáměstí staréMěstoZone

    let karlova = karlova staréMěstoZone

    let station =
        { Lines = [ Blue ]
          LeavesToStreet = staroměstskéNáměstí.Id
          PlaceId = metroStation.Id }

    staréMěstoZone
    |> World.Zone.addStreet (
        World.Node.create staroměstskéNáměstí.Id staroměstskéNáměstí
    )
    |> World.Zone.addStreet (World.Node.create karlova.Id karlova)
    |> World.Zone.connectStreets staroměstskéNáměstí.Id karlova.Id East

    |> World.Zone.addMetroStation station

module rec Duets.Data.World.Cities.Prague.StaréMěsto

open Duets.Data.World.Cities
open Duets.Entities

let staroměstskéNáměstí (zone: Zone) =
    let street =
        World.Street.create "Staroměstské náměstí" (StreetType.Split(South, 3))

    let bars =
        [ ("The Dubliner Irish Bar", 85<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let restaurants =
        [ ("U Prince", 92<quality>, Czech, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let hotels =
        [ ("Grand Hotel Praha", 93<quality>, 250m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    street
    |> World.Street.addPlaces bars
    |> World.Street.addPlaces restaurants
    |> World.Street.addPlaces hotels

let kaprova (zone: Zone) =
    let street = World.Street.create "Kaprova" (StreetType.OneWay)

    let metroStation =
        ("Staroměstská Station", zone.Id)
        |> (PlaceCreators.createMetro street.Id)

    let street = street |> World.Street.addPlace metroStation

    street, metroStation

let jilská (zone: Zone) =
    let street = World.Street.create "Jilská" (StreetType.OneWay)

    let concertSpaces =
        [ ("Jazz Republic",
           120,
           88<quality>,
           Layouts.concertSpaceLayout2,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    street |> World.Street.addPlaces concertSpaces

let dlouhá (zone: Zone) =
    let street = World.Street.create "Dlouhá" (StreetType.OneWay)

    let concertSpaces =
        [ ("Roxy Prague", 900, 90<quality>, Layouts.concertSpaceLayout3, zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    street |> World.Street.addPlaces concertSpaces

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

    let staroměstskéNáměstí = staroměstskéNáměstí staréMěstoZone

    let karlova = karlova staréMěstoZone
    let dlouhá = dlouhá staréMěstoZone
    let jilská = jilská staréMěstoZone
    let kaprova, metroStation = kaprova staréMěstoZone

    let station =
        { Lines = [ Blue ]
          LeavesToStreet = kaprova.Id
          PlaceId = metroStation.Id }

    staréMěstoZone
    |> World.Zone.addStreet (
        World.Node.create staroměstskéNáměstí.Id staroměstskéNáměstí
    )
    |> World.Zone.addStreet (World.Node.create karlova.Id karlova)
    |> World.Zone.addStreet (World.Node.create dlouhá.Id dlouhá)
    |> World.Zone.addStreet (World.Node.create jilská.Id jilská)
    |> World.Zone.addStreet (World.Node.create kaprova.Id kaprova)
    |> World.Zone.connectStreets staroměstskéNáměstí.Id karlova.Id East
    |> World.Zone.connectStreets staroměstskéNáměstí.Id dlouhá.Id North
    |> World.Zone.connectStreets staroměstskéNáměstí.Id jilská.Id South
    |> World.Zone.connectStreets staroměstskéNáměstí.Id kaprova.Id West

    |> World.Zone.addMetroStation station

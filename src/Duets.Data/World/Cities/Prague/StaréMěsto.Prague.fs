module rec Duets.Data.World.Cities.Prague.StaréMěsto

open Duets.Data.World.Cities.Prague
open Duets.Data.World.Cities
open Duets.Entities

let staroměstskéNáměstí (zone: Zone) =
    let street =
        World.Street.create
            Ids.Street.staroměstskéNáměstí
            (StreetType.Split(South, 3))
        |> World.Street.tag "pedestrian"
        |> World.Street.tag "astronomical-clock"

    let bars =
        [ ("The Dubliner Irish Bar", 85<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id >> World.Place.tag "pub")

    let restaurants =
        [ ("U Prince", 92<quality>, Czech, zone.Id)
          ("Staroměstská Restaurace", 84<quality>, Czech, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let hotels =
        [ ("Grand Hotel Praha", 93<quality>, 250m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    let cafes =
        [ ("Café Mozart", 88<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    street
    |> World.Street.addPlaces bars
    |> World.Street.addPlaces restaurants
    |> World.Street.addPlaces hotels
    |> World.Street.addPlaces cafes

let kaprova (zone: Zone) =
    let street =
        World.Street.create "Kaprova" (StreetType.OneWay)

    let metroStation =
        ("Staroměstská Station", zone.Id)
        |> (PlaceCreators.createMetro street.Id)

    let bars =
        [ ("La Bodeguita del Medio", 84<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id >> World.Place.tag "cocktail-bar")

    let cafes =
        [ ("Coffee Lovers", 82<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    let street =
        street
        |> World.Street.addPlaces bars
        |> World.Street.addPlaces cafes
        |> World.Street.addPlace metroStation

    street, metroStation

let jilská (zone: Zone) =
    let street =
        World.Street.create "Jilská" (StreetType.OneWay)

    let concertSpaces =
        [ ("Jazz Republic",
           120,
           88<quality>,
           Layouts.concertSpaceLayout2,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id >> World.Place.tag "jazz-club")

    street |> World.Street.addPlaces concertSpaces

let dlouhá (cityId: CityId) (zone: Zone) =
    let street =
        World.Street.create Ids.Street.dlouhá (StreetType.OneWay)

    let concertSpaces =
        [ ("Roxy Prague", 900, 90<quality>, Layouts.concertSpaceLayout3, zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let cinemas =
        [ ("Bio Oko", 85<quality>, zone.Id)
          ("Kino Světozor", 83<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCinema cityId street.Id)

    let cafes =
        [ ("Mamacoffee", 85<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    street
    |> World.Street.addPlaces concertSpaces
    |> World.Street.addPlaces cinemas
    |> World.Street.addPlaces cafes

let karlova (zone: Zone) =
    let street =
        World.Street.create Ids.Street.karlova (StreetType.Split(West, 2))
        |> World.Street.tag "charles-bridge-gate"

    let concertSpaces =
        [ ("Klementinum Mirror Chapel",
           200,
           94<quality>,
           Layouts.concertSpaceLayout4,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id >> World.Place.tag "seated")

    let bars =
        [ ("Anonymous Bar", 90<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id >> World.Place.tag "cocktail-bar")

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

let createZone (city: City) =
    let staréMěstoZone = World.Zone.create Ids.Zone.staréMěsto

    let staroměstskéNáměstí = staroměstskéNáměstí staréMěstoZone

    let karlova = karlova staréMěstoZone
    let dlouhá = dlouhá city.Id staréMěstoZone
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

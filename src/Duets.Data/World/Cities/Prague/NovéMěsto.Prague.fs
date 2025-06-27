module rec Duets.Data.World.Cities.Prague.NovéMěsto

open Duets.Data.World.Cities.Prague
open Duets.Data.World.Cities
open Duets.Entities

let václavskéNáměstí (zone: Zone) =
    let street =
        World.Street.create
            Ids.Street.václavskéNáměstí
            (StreetType.Split(North, 3))

    let bars =
        [ ("The Alchemist Bar", 92<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let restaurants =
        [ ("Výtopna Railway Restaurant", 88<quality>, Czech, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let hospitals =
        [ ("General University Hospital", 95<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createHospital street.Id)

    let metroStation =
        ("Můstek Station", zone.Id) |> (PlaceCreators.createMetro street.Id)

    let street =
        street
        |> World.Street.addPlaces bars
        |> World.Street.addPlaces restaurants
        |> World.Street.addPlaces hospitals
        |> World.Street.addPlace metroStation

    street, metroStation

let vodičkova (zone: Zone) =
    let street = World.Street.create "Vodičkova" StreetType.OneWay

    let concertSpaces =
        [ ("Lucerna Music Bar",
           800,
           90<quality>,
           Layouts.concertSpaceLayout1,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    street |> World.Street.addPlaces concertSpaces

let národní (zone: Zone) =
    let street = World.Street.create Ids.Street.národní StreetType.OneWay

    let concertSpaces =
        [ ("National Theatre",
           1000,
           95<quality>,
           Layouts.concertSpaceLayout4,
           zone.Id)
          ("Rock Café", 350, 88<quality>, Layouts.concertSpaceLayout2, zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let bars =
        [ ("Bukowski's Bar", 86<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let restaurants =
        [ ("Café Louvre", 90<quality>, French, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let hotels =
        [ ("Hotel Perla", 85<quality>, 120m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    street
    |> World.Street.addPlaces concertSpaces
    |> World.Street.addPlaces bars
    |> World.Street.addPlaces restaurants
    |> World.Street.addPlaces hotels

let zone =
    let novéMěstoZone = World.Zone.create Ids.Zone.novéMěsto

    let václavskéNáměstí, metroStation = václavskéNáměstí novéMěstoZone
    let národní = národní novéMěstoZone
    let vodičkova = vodičkova novéMěstoZone

    let station =
        { Lines = [ Red; Blue ]
          LeavesToStreet = václavskéNáměstí.Id
          PlaceId = metroStation.Id }

    novéMěstoZone
    |> World.Zone.addStreet (
        World.Node.create václavskéNáměstí.Id václavskéNáměstí
    )
    |> World.Zone.addStreet (World.Node.create národní.Id národní)
    |> World.Zone.addStreet (World.Node.create vodičkova.Id vodičkova)
    |> World.Zone.connectStreets václavskéNáměstí.Id národní.Id West
    |> World.Zone.connectStreets václavskéNáměstí.Id vodičkova.Id South

    |> World.Zone.addMetroStation station

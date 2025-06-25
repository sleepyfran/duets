module rec Duets.Data.World.Cities.Prague.Ruzyně

open Duets.Data.World.Cities
open Duets.Entities

let kLetišti (zone: Zone) =
    let street = World.Street.create "K Letišti" (StreetType.Split(North, 1))

    let airport =
        PlaceCreators.createAirport
            street.Id
            ("Václav Havel Airport Prague", 85<quality>, zone.Id)

    let metroStation =
        ("Nádraží Veleslavín Station", zone.Id)
        |> PlaceCreators.createMetro street.Id

    let concertSpaces =
        [ ("Airport Concert Hall",
           500,
           75<quality>,
           Layouts.concertSpaceLayout3,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let restaurants =
        [ ("Restaurace U Letiště", 78<quality>, Czech, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let hotels =
        [ ("Courtyard by Marriott Prague Airport",
           90<quality>,
           110m<dd>,
           zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    let street =
        street
        |> World.Street.addPlace airport
        |> World.Street.addPlace metroStation
        |> World.Street.addPlaces concertSpaces
        |> World.Street.addPlaces restaurants
        |> World.Street.addPlaces hotels

    street, metroStation

let zone =
    let ruzyněZone = World.Zone.create "Ruzyně"

    let kLetišti, metroStation = kLetišti ruzyněZone

    let station =
        { Lines = [ Blue ]
          LeavesToStreet = kLetišti.Id
          PlaceId = metroStation.Id }

    ruzyněZone
    |> World.Zone.addStreet (World.Node.create kLetišti.Id kLetišti)

    |> World.Zone.addMetroStation station

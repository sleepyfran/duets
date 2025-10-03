module Duets.Data.World.Cities.LosAngeles.DowntownLA

open Duets.Data.World.Cities
open Duets.Data.World.Cities.LosAngeles
open Duets.Entities

let figueroaStreet (zone: Zone) =
    let street =
        World.Street.create
            Ids.Street.figueroaStreet
            (StreetType.Split(North, 2))

    let concertSpaces =
        [ ("Crypto.com Arena",
           17000,
           98<quality>,
           Layouts.concertSpaceLayout4,
           zone.Id)
          ("Microsoft Theater",
           7100,
           94<quality>,
           Layouts.concertSpaceLayout4,
           zone.Id)
          ("Orpheum Theatre",
           2000,
           91<quality>,
           Layouts.concertSpaceLayout3,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let bars =
        [ ("W Hotel/L.A. Live Lounge", 93<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let rehearsalSpaces =
        [ ("Los Angeles Convention Center", 95<quality>, 500m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createRehearsalSpace street.Id)

    let hotels =
        [ ("The Grand Biltmore", 95<quality>, 500m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    let metroStation =
        ("7th St/Metro Center", zone.Id)
        |> (PlaceCreators.createMetro street.Id)

    let street =
        street
        |> World.Street.addPlaces concertSpaces
        |> World.Street.addPlaces bars
        |> World.Street.addPlaces rehearsalSpaces
        |> World.Street.addPlaces hotels
        |> World.Street.addPlace metroStation

    street, metroStation

let grandAvenue (zone: Zone) =
    let street = World.Street.create Ids.Street.grandAvenue StreetType.OneWay

    let concertSpaces =
        [ ("Walt Disney Concert Hall",
           2265,
           97<quality>,
           Layouts.concertSpaceLayout4,
           zone.Id)
          ("Dorothy Chandler Pavilion",
           3197,
           95<quality>,
           Layouts.concertSpaceLayout4,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let restaurants =
        [ ("Patina Restaurant", 94<quality>, French, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let cafes =
        [ ("Anya's Coffee", 90<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    street
    |> World.Street.addPlaces concertSpaces
    |> World.Street.addPlaces restaurants
    |> World.Street.addPlaces cafes

let zone =
    let downtownLAZone = World.Zone.create Ids.Zone.downtownLA

    let figueroaStreet, figueroaStreetStation = figueroaStreet downtownLAZone
    let grandAvenue = grandAvenue downtownLAZone

    let station =
        { Lines = [ Red; Blue ]
          LeavesToStreet = figueroaStreet.Id
          PlaceId = figueroaStreetStation.Id }

    downtownLAZone
    |> World.Zone.addStreet (World.Node.create figueroaStreet.Id figueroaStreet)
    |> World.Zone.addStreet (World.Node.create grandAvenue.Id grandAvenue)
    |> World.Zone.connectStreets figueroaStreet.Id grandAvenue.Id East
    |> World.Zone.addMetroStation station

module Duets.Data.World.Cities.LosAngeles.Lax

open Duets.Data.World.Cities.LosAngeles
open Duets.Data.World.Cities
open Duets.Entities

let centuryBoulevard (zone: Zone) =
    let street =
        World.Street.create Ids.Street.centuryBoulevard StreetType.OneWay

    let airports =
        [ ("Los Angeles International Airport", 95<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createAirport street.Id)

    let hotels =
        [ ("Century Park Suites", 75<quality>, 200m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    let metroStation =
        ("LAX/Metro Center", zone.Id) |> (PlaceCreators.createMetro street.Id)

    let street =
        street
        |> World.Street.addPlaces airports
        |> World.Street.addPlaces hotels
        |> World.Street.addPlace metroStation

    street, metroStation

let zone =
    let laxZone = World.Zone.create Ids.Zone.lax

    let centuryBoulevard, centuryBoulevardStation = centuryBoulevard laxZone

    let station =
        { Lines = [ Blue ]
          LeavesToStreet = centuryBoulevard.Id
          PlaceId = centuryBoulevardStation.Id }

    laxZone
    |> World.Zone.addStreet (
        World.Node.create centuryBoulevard.Id centuryBoulevard
    )
    |> World.Zone.addMetroStation station

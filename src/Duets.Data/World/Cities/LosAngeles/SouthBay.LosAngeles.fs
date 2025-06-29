module Duets.Data.World.Cities.LosAngeles.SouthBay

open Duets.Data.World.Cities.LosAngeles
open Duets.Data.World.Cities
open Duets.Entities

let centuryBoulevard (zone: Zone) =
    let street =
        World.Street.create
            Ids.Street.centuryBoulevard
            (StreetType.Split(East, 2))

    let airports =
        [ ("Los Angeles International Airport (LAX)", 95<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createAirport street.Id)

    let metroStation =
        ("LAX/City Center Station", zone.Id)
        |> (PlaceCreators.createMetro street.Id)

    let street =
        street
        |> World.Street.addPlaces airports
        |> World.Street.addPlace metroStation

    street, metroStation

let zone =
    let southBayZone = World.Zone.create Ids.Zone.southBay

    let centuryBoulevard, metroStation = centuryBoulevard southBayZone

    let station =
        { Lines = [ Red ]
          LeavesToStreet = centuryBoulevard.Id
          PlaceId = metroStation.Id }

    southBayZone
    |> World.Zone.addStreet (
        World.Node.create centuryBoulevard.Id centuryBoulevard
    )
    |> World.Zone.addMetroStation station

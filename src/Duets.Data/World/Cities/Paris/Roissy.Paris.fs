module Duets.Data.World.Cities.Paris.Roissy

open Duets.Data.World.Cities.Paris
open Duets.Data.World.Cities
open Duets.Entities

let avenueCharlesDeGaulle (zone: Zone) =
    let street =
        World.Street.create Ids.Street.avenueCharlesDeGaulle StreetType.OneWay

    let airports =
        [ ("Charles de Gaulle Airport", 90<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createAirport street.Id)

    let hotels =
        [ ("Sheraton Paris Charles de Gaulle", 85<quality>, 280m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    let metroStation =
        ("CDG Airport Station", zone.Id)
        |> (PlaceCreators.createMetro street.Id)

    let street =
        street
        |> World.Street.addPlaces airports
        |> World.Street.addPlaces hotels
        |> World.Street.addPlace metroStation

    street, metroStation

let zone =
    let roissyZone = World.Zone.create Ids.Zone.roissy

    let avenueCharlesDeGaulle, cdgStation = avenueCharlesDeGaulle roissyZone

    let station =
        { Lines = [ Blue ]
          LeavesToStreet = avenueCharlesDeGaulle.Id
          PlaceId = cdgStation.Id }

    roissyZone
    |> World.Zone.addStreet (
        World.Node.create avenueCharlesDeGaulle.Id avenueCharlesDeGaulle
    )
    |> World.Zone.addMetroStation station

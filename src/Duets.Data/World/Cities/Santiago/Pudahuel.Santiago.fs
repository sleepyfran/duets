module Duets.Data.World.Cities.Santiago.Pudahuel

open Duets.Data.World.Cities.Santiago
open Duets.Data.World.Cities
open Duets.Entities

let ruta68 (zone: Zone) =
    let street =
        World.Street.create Ids.Street.ruta68 StreetType.OneWay

    let airports =
        [ ("Aeropuerto Internacional Arturo Merino Benítez", 90<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createAirport street.Id)

    let hotels =
        [ ("Marriott Santiago Airport", 86<quality>, 220m<dd>, zone.Id)
          ("Holiday Inn Express Santiago Airport", 78<quality>, 150m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    let metroStation =
        ("Pudahuel Metro Station", zone.Id) |> PlaceCreators.createMetro street.Id

    let street =
        street
        |> World.Street.addPlaces airports
        |> World.Street.addPlaces hotels
        |> World.Street.addPlace metroStation

    street, metroStation

let zone =
    let pudahuelZone = World.Zone.create Ids.Zone.pudahuel

    let ruta68, pudahuelMetroStation = ruta68 pudahuelZone

    let station =
        { Lines = [ Blue ]
          LeavesToStreet = ruta68.Id
          PlaceId = pudahuelMetroStation.Id }

    pudahuelZone
    |> World.Zone.addStreet (World.Node.create ruta68.Id ruta68)
    |> World.Zone.addMetroStation station

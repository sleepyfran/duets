module rec Duets.Data.World.Cities.NewYork.Jamaica

open Duets.Data.World.Cities
open Duets.Entities

let private vanWyckExpressway (zone: Zone) =
    let street =
        World.Street.create
            Ids.Street.vanWyckExpressway
            (StreetType.Split(North, 2))

    let airports =
        [ ("John F. Kennedy International Airport", 95<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createAirport street.Id)

    let metroStation =
        ("JFK Airport AirTrain", zone.Id) |> PlaceCreators.createMetro street.Id

    let street =
        street
        |> World.Street.addPlaces airports
        |> World.Street.addPlace metroStation

    street, metroStation

let zone =
    let jamaicaZone = World.Zone.create Ids.Zone.jamaica

    let vanWyckExpressway, jfkMetro = vanWyckExpressway jamaicaZone

    let jfkMetroStation =
        { Lines = [ Blue ]
          LeavesToStreet = vanWyckExpressway.Id
          PlaceId = jfkMetro.Id }

    jamaicaZone
    |> World.Zone.addStreet (
        World.Node.create vanWyckExpressway.Id vanWyckExpressway
    )
    |> World.Zone.addMetroStation jfkMetroStation

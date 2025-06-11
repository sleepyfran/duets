module rec Duets.Data.World.Cities.LosAngeles.LAX

open Duets.Data.World.Cities
open Duets.Entities

let zone =
    let laxZone = World.Zone.create "LAX"

    let airportStreet =
        World.Street.create "World Way" (StreetType.Split(North, 1))

    let airport =
        PlaceCreators.createAirport
            airportStreet.Id
            ("Los Angeles International Airport", 90<quality>, laxZone.Id)

    let metroStation =
        ("LAX/Metro Transit Center", laxZone.Id)
        |> PlaceCreators.createMetro airportStreet.Id

    let airportStreet =
        airportStreet
        |> World.Street.addPlace airport
        |> World.Street.addPlace metroStation

    let station =
        { Lines = [ Blue ]
          LeavesToStreet = airportStreet.Id
          PlaceId = metroStation.Id }

    laxZone
    |> World.Zone.addStreet (World.Node.create airportStreet.Id airportStreet)
    |> World.Zone.addDescriptor Industrial
    |> World.Zone.addMetroStation station

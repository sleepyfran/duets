module rec Duets.Data.World.Cities.Tokyo.Haneda

open Duets.Data.World.Cities
open Duets.Entities
open Duets.Entities.Calendar

let zone =
    let hanedaZone = World.Zone.create "Haneda"

    let airportStreet =
        World.Street.create "Haneda Airport Road" (StreetType.Split(North, 1))

    let airport =
        PlaceCreators.createAirport
            airportStreet.Id
            ("Tokyo International Airport (Haneda)", 90<quality>, hanedaZone.Id)

    let metroStation =
        ("Haneda Airport Station", hanedaZone.Id)
        |> PlaceCreators.createMetro airportStreet.Id

    let airportStreet =
        airportStreet
        |> World.Street.addPlace airport
        |> World.Street.addPlace metroStation

    let observationStreet =
        World.Street.create "Observation Deck Road" StreetType.OneWay

    let cafes =
        [ ("Haneda Sky Cafe", 78<quality>, hanedaZone.Id) ]
        |> List.map (PlaceCreators.createCafe observationStreet.Id)

    let observationStreet =
        observationStreet
        |> World.Street.addPlaces cafes

    let station =
        { Lines = [ Blue ]
          LeavesToStreet = airportStreet.Id
          PlaceId = metroStation.Id }

    hanedaZone
    |> World.Zone.addStreet (World.Node.create airportStreet.Id airportStreet)
    |> World.Zone.addStreet (World.Node.create observationStreet.Id observationStreet)
    |> World.Zone.addDescriptor Industrial
    |> World.Zone.addMetroStation station
    |> World.Zone.connectStreets airportStreet.Id observationStreet.Id East

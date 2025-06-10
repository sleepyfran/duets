module rec Duets.Data.World.Cities.Prague.Ruzyně

open Duets.Data.World
open Duets.Data.World.Cities
open Duets.Entities

let zone =
    let ruzyněZone = World.Zone.create "Ruzyně"

    let airportStreet =
        World.Street.create "Letecká" (StreetType.Split(North, 1))

    let airport =
        World.Place.create
            "Letiště Václava Havla Praha"
            85<quality>
            Airport
            Layouts.airportLayout
            ruzyněZone.Id
        |> World.Place.addExit Ids.Common.lobby airportStreet.Id

    let metroStation =
        ("Letiště Station", ruzyněZone.Id)
        |> PlaceCreators.createMetro airportStreet.Id

    let airportStreet =
        airportStreet
        |> World.Street.addPlace airport
        |> World.Street.addPlace metroStation

    let station =
        { Lines = [ Blue ]
          LeavesToStreet = airportStreet.Id
          PlaceId = metroStation.Id }

    ruzyněZone
    |> World.Zone.addStreet (World.Node.create airportStreet.Id airportStreet)
    |> World.Zone.addDescriptor Industrial
    |> World.Zone.addMetroStation station

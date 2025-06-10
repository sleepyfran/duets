module rec Duets.Data.World.Cities.Prague.Vršovice

open Duets.Data.World.Cities
open Duets.Entities

let createZone city =
    let vršoviceZone = World.Zone.create "Vršovice"

    let vršovická (zone: Zone) =
        let street = World.Street.create "Vršovická" (StreetType.Split(East, 1))

        let concertSpaces =
            [ ("Eden Aréna",
               21000,
               90<quality>,
               Layouts.concertSpaceLayout1,
               zone.Id) ]
            |> List.map (PlaceCreators.createConcertSpace street.Id)

        street |> World.Street.addPlaces concertSpaces

    let vršovická = vršovická vršoviceZone

    vršoviceZone
    |> World.Zone.addStreet (World.Node.create vršovická.Id vršovická)
    |> World.Zone.addDescriptor Industrial

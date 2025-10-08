module Duets.Entities.Tests.World

open FsUnit
open NUnit.Framework

open Duets.Entities

let testZone = World.Zone.create "Test Zone"

let private singleStreetOfZone zone =
    zone.Streets |> World.Graph.nodes |> List.head

[<Test>]
let ``adding any street to a zone creates a synthetic place with that street``
    ()
    =
    [ StreetType.OneWay; StreetType.Split(West, 2) ]
    |> List.iter (fun streetType ->
        let street = World.Street.create "Test Street" streetType

        let zone =
            testZone
            |> World.Zone.addStreet (World.Node.create street.Id street)

        let updatedStreet = singleStreetOfZone zone

        updatedStreet.Places |> should haveLength 1

        let streetPlace = updatedStreet.Places |> List.head
        streetPlace.PlaceType |> should be (ofCase <@ PlaceType.Street @>))

[<Test>]
let ``adding a one-way street to a zone creates a single room node inside`` () =
    let oneWayStreet = World.Street.create "One Way Street" StreetType.OneWay

    let zone =
        testZone
        |> World.Zone.addStreet (World.Node.create oneWayStreet.Id oneWayStreet)

    let updatedStreet = singleStreetOfZone zone

    let streetPlace = updatedStreet.Places |> List.head
    let streetRoom = streetPlace.Rooms |> World.Graph.nodes |> List.head

    streetRoom.RoomType |> should be (ofCase <@ RoomType.Street @>)

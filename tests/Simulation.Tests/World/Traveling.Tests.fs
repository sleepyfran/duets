module Duets.Simulation.Tests.World.Traveling

open Duets.Data.World
open FsUnit
open NUnit.Framework
open Test.Common

open Duets.Entities
open Duets.Simulation


let private hotelInPrague =
    Queries.World.placesByTypeInCity Prague PlaceTypeIndex.Hotel |> List.head

let private hospitalInPrague =
    Queries.World.placesByTypeInCity Prague PlaceTypeIndex.Hospital |> List.head

let private hotelInNewYork =
    Queries.World.placesByTypeInCity NewYork PlaceTypeIndex.Hotel |> List.head

let private bookstoreInSohoNewYork =
    Queries.World.placesByTypeInCity NewYork PlaceTypeIndex.Bookstore
    |> List.find (fun place ->
        place.ZoneId = Identity.Reproducible.create "SoHo")

let private cafeInSohoNewYork =
    Queries.World.placesByTypeInCity NewYork PlaceTypeIndex.Cafe
    |> List.find (fun place ->
        place.ZoneId = Identity.Reproducible.create "SoHo")

[<Test>]
let ``traveling to another place inside the same city but in another region consumes 25 minutes``
    ()
    =
    let effects =
        WorldMoveToPlace(
            Diff(
                (Prague, hotelInPrague.Id, Ids.Common.lobby),
                (Prague, hospitalInPrague.Id, Ids.Common.lobby)
            )
        )
        |> Simulation.tickOne dummyState

    effects |> fst |> should contain (TurnTimeUpdated 25<minute>)

[<Test>]
let ``traveling to another place inside the same city but in the same region consumes 10 minutes``
    ()
    =
    let effects =
        WorldMoveToPlace(
            Diff(
                (NewYork, bookstoreInSohoNewYork.Id, Ids.Bookstore.readingRoom),
                (NewYork, cafeInSohoNewYork.Id, Ids.Common.cafe)
            )
        )
        |> Simulation.tickOne dummyState

    effects |> fst |> should contain (TurnTimeUpdated 10<minute>)

[<Test>]
let ``traveling to another city does not add 25 minutes on top of flight time``
    ()
    =
    let effects =
        WorldMoveToPlace(
            Diff(
                (Prague, hotelInPrague.Id, Ids.Common.lobby),
                (NewYork, hotelInNewYork.Id, Ids.Common.lobby)
            )
        )
        |> Simulation.tickOne dummyState

    effects |> fst |> should not' (contain (TurnTimeUpdated 30<minute>))

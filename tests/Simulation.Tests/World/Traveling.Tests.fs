module Duets.Simulation.Tests.World.Traveling

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
    |> List.find (fun place -> place.Zone.Name = "SoHo")

let private cafeInSohoNewYork =
    Queries.World.placesByTypeInCity NewYork PlaceTypeIndex.Cafe
    |> List.find (fun place -> place.Zone.Name = "SoHo")

[<Test>]
let ``traveling to another place inside the same city but in another region consumes 30 minutes``
    ()
    =
    let effects =
        WorldMoveToPlace(
            Diff(
                (Prague, hotelInPrague.Id, 1),
                (Prague, hospitalInPrague.Id, 1)
            )
        )
        |> Simulation.tickOne dummyState

    effects |> fst |> should contain (TurnTimeUpdated 30<minute>)

[<Test>]
let ``traveling to another place inside the same city but in the same region consumes 15 minutes``
    ()
    =
    let effects =
        WorldMoveToPlace(
            Diff(
                (NewYork, bookstoreInSohoNewYork.Id, 1),
                (NewYork, cafeInSohoNewYork.Id, 1)
            )
        )
        |> Simulation.tickOne dummyState

    effects |> fst |> should contain (TurnTimeUpdated 15<minute>)

[<Test>]
let ``traveling to another city does not add 30 minutes on top of flight time``
    ()
    =
    let effects =
        WorldMoveToPlace(
            Diff((Prague, hotelInPrague.Id, 1), (NewYork, hotelInNewYork.Id, 1))
        )
        |> Simulation.tickOne dummyState

    effects |> fst |> should not' (contain (TurnTimeUpdated 30<minute>))

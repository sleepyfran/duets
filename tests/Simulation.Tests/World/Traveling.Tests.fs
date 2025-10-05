module Duets.Simulation.Tests.World.Traveling

open FsUnit
open NUnit.Framework
open Test.Common

open Duets.Entities
open Duets.Simulation
open Duets.Data.World
open Duets.Data.World.Cities


let private hotelInPrague =
    Queries.World.placesByTypeInCity Prague PlaceTypeIndex.MetroStation
    |> List.head

let private hospitalInPrague =
    Queries.World.placesByTypeInCity Prague PlaceTypeIndex.Hospital |> List.head

let private hotelInNewYork =
    Queries.World.placesByTypeInCity NewYork PlaceTypeIndex.Hotel |> List.head

let private barInBrooklynNewYork =
    Queries.World.placesByTypeInCity NewYork PlaceTypeIndex.Bar
    |> List.find (fun place ->
        place.ZoneId = Identity.Reproducible.create NewYork.Ids.Zone.brooklyn)

let private cafeInBrooklynNewYork =
    Queries.World.placesByTypeInCity NewYork PlaceTypeIndex.MetroStation
    |> List.find (fun place ->
        place.ZoneId = Identity.Reproducible.create NewYork.Ids.Zone.brooklyn)

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
                (NewYork, barInBrooklynNewYork.Id, Ids.Bookstore.readingRoom),
                (NewYork, cafeInBrooklynNewYork.Id, Ids.Common.cafe)
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

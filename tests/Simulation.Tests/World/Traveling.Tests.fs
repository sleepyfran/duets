module Duets.Simulation.Tests.World.Traveling

open FsUnit
open NUnit.Framework
open Test.Common

open Duets.Entities
open Duets.Simulation


let private hotelInPrague =
    Queries.World.placesByTypeInCity Prague PlaceTypeIndex.Hotel |> List.head

let private barInPrague =
    Queries.World.placesByTypeInCity Prague PlaceTypeIndex.Bar |> List.head

let private hotelInNewYork =
    Queries.World.placesByTypeInCity NewYork PlaceTypeIndex.Hotel |> List.head

[<Test>]
let ``traveling to another place inside the same city consumes 30 minutes`` () =
    let effects =
        WorldMoveToPlace(
            Diff((Prague, hotelInPrague.Id, 1), (Prague, barInPrague.Id, 1))
        )
        |> Simulation.tickOne dummyState

    effects |> fst |> should contain (TurnTimeUpdated 30<minute>)

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

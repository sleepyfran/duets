module Duets.Simulation.Tests.Market.GenreMarket

open FsUnit
open NUnit.Framework

open Duets.Entities
open Duets.Simulation.Market

let unwrap =
    function
    | GenreMarketsUpdated market -> market
    | _ -> failwith "Unexpected effect"

let checkMarketPointBoundaries market =
    market.MarketPoint |> should be (greaterThanOrEqualTo 0.1)

    market.MarketPoint |> should be (lessThanOrEqualTo 5.0)

[<Test>]
let ``create should generate a random genre market for each of the given genres within the correct range``
    ()
    =
    let markets =
        GenreMarket.create
            [ for x in 1..500 do
                  yield $"Test {x}" ]

    markets |> should haveCount 500

    markets
    |> Map.iter (fun _ market ->
        checkMarketPointBoundaries market

        market.Fluctuation |> should be (greaterThanOrEqualTo 0.1)

        market.Fluctuation |> should be (lessThanOrEqualTo 1.1))

[<Test>]
let ``update should modify each market within the fluctuation keeping it within the correct range``
    ()
    =
    let genreMarkets =
        [ "Test 1", { MarketPoint = 3.3; Fluctuation = 0.5 }
          "Test 2", { MarketPoint = 4.5; Fluctuation = 1.1 }
          "Test 3", { MarketPoint = 0.5; Fluctuation = 0.2 } ]
        |> Map.ofList

    // Since the update relies on a random boolean, spin it a bunch of times.
    for _ in 1..100 do
        GenreMarket.update genreMarkets
        |> unwrap
        |> Map.iter (fun _ -> checkMarketPointBoundaries)

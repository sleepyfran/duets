module Duets.Entities.Tests.Genre

open FsUnit
open NUnit.Framework

open Duets.Entities

let private floatBetween min _ = min

[<Test>]
let ``Market.create creates a market for each genre`` () =
    Genre.Market.create floatBetween [ "Rock"; "Jazz" ]
    |> should
        equal
        (Map.ofList
            [ "Rock", { MarketPoint = 0.1; Fluctuation = 0.1 }
              "Jazz", { MarketPoint = 0.1; Fluctuation = 0.1 } ])

[<Test>]
let ``Market.addMissing creates markets for genres without one`` () =
    Genre.Market.addMissing floatBetween [ "Rock"; "Jazz" ] Map.empty
    |> Map.keys
    |> List.ofSeq
    |> should equal [ "Jazz"; "Rock" ]

[<Test>]
let ``Market.addMissing keeps existing markets as they are`` () =
    let rockMarket = { MarketPoint = 4.2; Fluctuation = 1.0 }

    Genre.Market.addMissing
        floatBetween
        [ "Rock"; "Jazz" ]
        (Map.ofList [ "Rock", rockMarket ])
    |> Map.find "Rock"
    |> should equal rockMarket

module Duets.Simulation.Tests.Merchandise.Sell

open NUnit.Framework
open FsUnit
open Test.Common

open Duets.Entities
open Duets.Simulation

let private dummyVinylMerch =
    { Brand = "DuetsMerch"
      Name = "Vinyl"
      Properties = [ Listenable(Vinyl, dummyAlbum.Id) ] }

let private dummyTShirtMerch =
    { Brand = "DuetsMerch"
      Name = "T-shirt"
      Properties = [ Wearable TShirt ] }

let private headlinerConcert = { dummyConcert with TicketsSold = 1000 }

let private openingActConcert =
    { dummyConcert with
        TicketsSold = 1000
        ParticipationType = OpeningAct(dummyBand.Id, 5<percent>) }

let stateWithMerch =
    dummyState
    |> State.Merch.setPrice dummyBand.Id dummyVinylMerch 15m<dd>
    |> State.Merch.setPrice dummyBand.Id dummyTShirtMerch 15m<dd>
    |> State.Inventory.addToBand dummyTShirtMerch 200<quantity>
    |> State.Inventory.addToBand dummyVinylMerch 150<quantity>

[<Test>]
let ``Sell.afterConcert does nothing if band has no merch`` () =
    let concert = { dummyConcert with TicketsSold = 1000 }
    let performedConcert = PerformedConcert(concert, 100<quality>)

    Merchandise.Sell.afterConcert dummyBand performedConcert dummyState
    |> should haveLength 0

[<Test>]
let ``Sell.afterConcert does nothing if nobody came to the concert`` () =
    let concert = { dummyConcert with TicketsSold = 0 }
    let performedConcert = PerformedConcert(concert, 100<quality>)

    Merchandise.Sell.afterConcert dummyBand performedConcert dummyState
    |> should haveLength 0

let private checkExpectedSales state concert quality expectedItems =
    let performedConcert = PerformedConcert(concert, quality)

    let effect =
        Merchandise.Sell.afterConcert dummyBand performedConcert state
        |> List.head

    match effect with
    | MerchSold(_, soldItems, _) ->
        soldItems |> List.sumBy snd |> should equal expectedItems
    | _ -> failwith "Unexpected effect"

/// --------------------
/// Concert quality
/// --------------------

[<Test>]
let ``Sell.afterConcert sells poorly if quality of concert was less than 25``
    ()
    =
    checkExpectedSales stateWithMerch headlinerConcert 20<quality> 1

[<Test>]
let ``Sell.afterConcert sells okay if quality of concert was less than 50`` () =
    checkExpectedSales stateWithMerch headlinerConcert 45<quality> 50

[<Test>]
let ``Sell.afterConcert sells well if quality of concert was between 50 and 75``
    ()
    =
    checkExpectedSales stateWithMerch headlinerConcert 70<quality> 150

[<Test>]
let ``Sell.afterConcert sells really well if quality of concert was more than 75``
    ()
    =
    checkExpectedSales stateWithMerch headlinerConcert 99<quality> 300

[<Test>]
let ``Sell.afterConcert reduces sales to 1.5% of the expected ones if participation type is opening act``
    ()
    =
    checkExpectedSales stateWithMerch openingActConcert 99<quality> 5

/// --------------------
/// Merch limits
/// --------------------

[<Test>]
let ``Sell.afterConcert limits the sales to the stock of merch the band has``
    ()
    =
    let state =
        dummyState
        |> State.Merch.setPrice dummyBand.Id dummyVinylMerch 15m<dd>
        |> State.Inventory.addToBand dummyVinylMerch 100<quantity>

    checkExpectedSales state headlinerConcert 99<quality> 100

/// --------------------
/// Merch price
/// --------------------

[<Test>]
let ``Sell.afterConcert reduces the sales to 50% if price is more than recommended but less than 10% more``
    ()
    =
    let stateWithOverpricedMerch =
        dummyState
        |> State.Merch.setPrice dummyBand.Id dummyTShirtMerch 16m<dd>
        |> State.Inventory.addToBand dummyTShirtMerch 200<quantity>

    checkExpectedSales stateWithOverpricedMerch headlinerConcert 99<quality> 100

[<Test>]
let ``Sell.afterConcert reduces the sales to 10% if price is more than 10% of the recommended price but less than 50% more``
    ()
    =
    let stateWithOverpricedMerch =
        dummyState
        |> State.Merch.setPrice dummyBand.Id dummyTShirtMerch 21m<dd>
        |> State.Inventory.addToBand dummyTShirtMerch 200<quantity>

    checkExpectedSales stateWithOverpricedMerch headlinerConcert 99<quality> 20

[<Test>]
let ``Sell.afterConcert sells nothing if price is more than than 50% of the recommended price``
    ()
    =
    let stateWithOverpricedMerch =
        dummyState
        |> State.Merch.setPrice dummyBand.Id dummyTShirtMerch 25m<dd>
        |> State.Inventory.addToBand dummyTShirtMerch 200<quantity>

    checkExpectedSales stateWithOverpricedMerch headlinerConcert 99<quality> 0

/// --------------------
/// Earnings
/// --------------------

[<Test>]
let ``Sell.afterConcert generates income effect to the band account for all sold items``
    ()
    =
    let performedConcert = PerformedConcert(headlinerConcert, 99<quality>)

    Merchandise.Sell.afterConcert dummyBand performedConcert stateWithMerch
    |> List.iter (function
        | MerchSold(_, _, earnings) -> earnings |> should equal 4500m<dd>
        | MoneyEarned(Band bandId, Incoming(amount, balance)) ->
            bandId |> should equal dummyBand.Id
            amount |> should equal 4500m<dd>
            balance |> should equal 4500m<dd>
        | _ -> failwith "Unexpected effect")

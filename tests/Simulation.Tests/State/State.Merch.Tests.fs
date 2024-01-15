module Duets.Simulation.Tests.State.Merch

open FsUnit
open NUnit.Framework
open Test.Common

open Duets.Entities
open Duets.Simulation

let private dummyMerch =
    { Brand = "DuetsMerch"
      Name = "T-shirt"
      Properties = [ Wearable TShirt ] }

[<Test>]
let ``MerchSold updates the band inventory removing the sold items`` () =
    let state =
        ItemAddedToBandInventory(dummyMerch, 200<quantity>)
        |> State.Root.applyEffect dummyState

    let state =
        MerchSold(dummyBand, [ dummyMerch, 91<quantity> ], 200m<dd>)
        |> State.Root.applyEffect state

    Queries.Inventory.band state
    |> Map.find dummyMerch
    |> should equal 109<quantity>

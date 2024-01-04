module Duets.Simulation.Tests.State.Inventory

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
let ``ItemAddedToBandInventory adds the item to the inventory`` () =
    let state =
        ItemAddedToBandInventory(dummyMerch, 200<quantity>)
        |> State.Root.applyEffect dummyState

    Queries.Inventory.band state
    |> Map.find dummyMerch
    |> should equal 200<quantity>

[<Test>]
let ``ItemAddedToInventory sums the new quantity to the previous if the item was already in the map``
    ()
    =
    let state =
        ItemAddedToBandInventory(dummyMerch, 200<quantity>)
        |> State.Root.applyEffect dummyState

    let state =
        ItemAddedToBandInventory(dummyMerch, 100<quantity>)
        |> State.Root.applyEffect state

    Queries.Inventory.band state
    |> Map.find dummyMerch
    |> should equal 300<quantity>

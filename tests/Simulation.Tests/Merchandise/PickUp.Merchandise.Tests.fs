module Duets.Simulation.Tests.Merchandise.PickUp

open NUnit.Framework
open FsUnit
open Test.Common

open Duets.Entities
open Duets.Simulation.Merchandise.PickUp

let private dummyMerch =
    { Brand = "DuetsMerch"
      Name = "T-shirt"
      Properties = [ Wearable TShirt ] }

let private deliverable =
    { Brand = ""
      Name = ""
      Properties =
        [ Deliverable(
              dummyToday,
              DeliverableItem.Description(dummyMerch, 100<quantity>)
          ) ] }

[<Test>]
let ``pick-up does nothing if the items are empty`` () =
    pickUpOrder dummyState [] |> should haveLength 0

[<Test>]
let ``pick-up ignores non-deliverable items`` () =
    pickUpOrder dummyState [ dummyMerch ] |> should haveLength 0

[<Test>]
let ``pick-up adds the merch to the band inventory`` () =
    pickUpOrder dummyState [ deliverable ]
    |> List.filter (function
        | ItemAddedToBandInventory(item, quantity) ->
            item = dummyMerch && quantity = 100<quantity>
        | _ -> false)
    |> should haveLength 1

[<Test>]
let ``pick-up removes the item from the world`` () =
    pickUpOrder dummyState [ deliverable ]
    |> List.filter (function
        | ItemRemovedFromWorld(_, item) -> item = deliverable
        | _ -> false)
    |> should haveLength 1

module Duets.Simulation.Tests.Events.World

open FsUnit
open NUnit.Framework
open Test.Common
open Test.Common.Generators

open Duets.Common
open Duets.Data
open Duets.Entities
open Duets.Simulation

let private gym =
    Queries.World.placesByTypeInCity Prague PlaceTypeIndex.Gym |> List.head

let private requiredItems =
    gym.Rooms.Nodes
    |> Map.find World.Ids.Gym.changingRoom
    |> fun room ->
        room.RequiredItemsForEntrance
        |> Option.get
        |> fun requiredItems -> requiredItems.Items

let private stateWithItems =
    let state = State.generateOne State.defaultOptions

    requiredItems
    |> List.fold (fun state item -> State.Inventory.add item state) state

[<Test>]
let ``tick of world enter should remove required items from inventory if movement is towards the room in ComingFrom field``
    ()
    =
    let effect =
        WorldEnter(
            Diff(
                (Prague, gym.Id, World.Ids.Gym.changingRoom),
                (Prague, gym.Id, World.Ids.Gym.lobby)
            )
        )

    Simulation.tickOne stateWithItems effect
    |> fst
    |> List.item 1
    |> should be (ofCase <@ ItemRemovedFromInventory @>)

[<Test>]
let ``tick of world move should remove required items from inventory if moving from a place that has any room with required items`` =
    let effect =
        WorldMoveTo(
            Diff(
                (Prague, gym.Id, World.Ids.Gym.changingRoom),
                (Prague, dummyPlace.Id, 0)
            )
        )

    Simulation.tickOne stateWithItems effect
    |> fst
    |> List.item 1
    |> should be (ofCase <@ ItemRemovedFromInventory @>)

[<Test>]
let ``tick of world enter should not remove any items from inventory if movement is towards any other room``
    ()
    =
    let effect =
        WorldEnter(
            Diff(
                (Prague, gym.Id, World.Ids.Gym.changingRoom),
                (Prague, gym.Id, World.Ids.Gym.gym)
            )
        )

    Simulation.tickOne stateWithItems effect
    |> fst
    |> List.filter (function
        | ItemRemovedFromInventory _ -> true
        | _ -> false)
    |> should haveLength 0

module Duets.Simulation.Tests.Items.Remove

open FsUnit
open NUnit.Framework
open Test.Common

open Duets.Data.Items
open Duets.Entities
open Duets.Simulation

let private dummyItem = Food.Index.all |> List.head |> fst

let private dummyPlace =
    Queries.World.placesByTypeInCity Prague PlaceTypeIndex.Bar |> List.head

[<Test>]
let ``remove does nothing if the item is not in world or inventory`` () =
    Items.remove dummyState dummyItem |> should haveLength 0

[<Test>]
let ``remove removes from world if it in the current location`` () =
    let currentLocation = Prague, dummyPlace.Id, "0"

    let state =
        { dummyState with
            CurrentPosition = currentLocation
            WorldItems = [ currentLocation, [ dummyItem ] ] |> Map.ofList }

    Items.remove state dummyItem
    |> List.head
    |> should be (ofCase <@ ItemRemovedFromWorld @>)

[<Test>]
let ``remove removes form inventory if the character has it there`` () =
    let state =
        { dummyState with
            Inventories =
                { Character = [ dummyItem ]
                  Band = Map.empty } }

    Items.remove state dummyItem
    |> List.head
    |> should be (ofCase <@ ItemRemovedFromCharacterInventory @>)

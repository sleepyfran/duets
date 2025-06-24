module Duets.Simulation.Tests.Items.Remove

open Duets.Data.World
open FsUnit
open NUnit.Framework
open Test.Common.Generators

open Duets.Data.Items
open Duets.Entities
open Duets.Simulation

let private item = Food.Index.all |> List.head |> fst

let private bar =
    Queries.World.placesByTypeInCity Prague PlaceTypeIndex.Bar |> List.head

let private state = State.generateOne State.defaultOptions

[<Test>]
let ``remove does nothing if the item is not in world or inventory`` () =
    Items.remove state item |> should haveLength 0

[<Test>]
let ``remove removes from world if it in the current location`` () =
    let currentLocation = Prague, bar.Id, Ids.Common.bar

    let state =
        { state with
            CurrentPosition = currentLocation
            WorldItems = [ currentLocation, [ item ] ] |> Map.ofList }

    Items.remove state item
    |> List.head
    |> should be (ofCase <@ ItemRemovedFromWorld @>)

[<Test>]
let ``remove removes form inventory if the character has it there`` () =
    let state =
        { state with
            Inventories =
                { Character = [ item ]
                  Band = Map.empty } }

    Items.remove state item
    |> List.head
    |> should be (ofCase <@ ItemRemovedFromCharacterInventory @>)

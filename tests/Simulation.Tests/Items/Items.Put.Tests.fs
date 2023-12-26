module Duets.Simulation.Tests.Items.Put

open FsUnit
open NUnit.Framework
open Test.Common

open Duets.Entities
open Duets.Common
open Duets.Data.Items
open Duets.Simulation

let nonPlaceableItem = Food.Index.all |> List.head |> fst

let placeableItem = Book.all |> List.head |> fst

let shelf = Furniture.Storage.ikeaShelf |> fst

[<Test>]
let ``put item fails if item is not placeable`` () =
    Items.place dummyState nonPlaceableItem shelf
    |> Result.unwrapError
    |> should be (ofCase <@ Items.ItemIsNotPlaceable @>)

[<Test>]
let ``put item fails if storage item is not capable of storing`` () =
    Items.place dummyState nonPlaceableItem nonPlaceableItem
    |> Result.unwrapError
    |> should be (ofCase <@ Items.StorageItemIsNotStorage @>)

[<Test>]
let ``put item creates a item changed in world effect`` () =
    let effect =
        Items.place dummyState placeableItem shelf |> Result.unwrap |> List.head

    match effect with
    | ItemChangedInWorld(coords, Diff(_, curr)) ->
        match curr.Properties with
        | [ Storage(_, items) ] -> items |> should contain placeableItem
        | _ -> failwith "Unexpected property"

        coords |> should equal dummyState.CurrentPosition
    | _ -> failwith "Unexpected effect"

[<Test>]
let ``put item creates a item removed from inventory effect`` () =
    let effect =
        Items.place dummyState placeableItem shelf
        |> Result.unwrap
        |> List.item 1

    match effect with
    | ItemRemovedFromInventory(_, item) -> item |> should equal placeableItem
    | _ -> failwith "Unexpected effect"

module Duets.Simulation.Tests.Items.Take

open FsUnit
open NUnit.Framework
open Test.Common

open Duets.Common
open Duets.Entities
open Duets.Data.Items
open Duets.Simulation

let placeableItems = Book.all |> List.take 5 |> List.map fst

let placeableItem = placeableItems |> List.head

let shelf = Furniture.Storage.ikeaShelf |> fst

[<Test>]
let ``from returns the items stored in item if it's storage`` () =
    let shelfWithItems =
        { shelf with
            Properties = [ Storage(Shelf, placeableItems) ] }

    let items = Items.from shelfWithItems
    items |> Result.unwrap |> should equal placeableItems

[<Test>]
let ``take returns error if the given item is not storage`` () =
    Items.take dummyState placeableItem placeableItem
    |> Result.unwrapError
    |> should be (ofCase <@ Items.StorageItemIsNotStorage @>)

[<Test>]
let ``take updates storage to remove the request item`` () =
    let dummyCoords = Queries.World.currentCoordinates dummyState

    let shelfWithItems =
        { shelf with
            Properties = [ Storage(Shelf, placeableItems) ] }

    let effect =
        Items.take dummyState placeableItem shelfWithItems
        |> Result.unwrap
        |> List.head

    match effect with
    | ItemChangedInWorld(coords, Diff(_, updatedShelf)) ->
        coords |> should equal dummyCoords

        updatedShelf.Properties
        |> should equal [ Storage(Shelf, placeableItems |> List.tail) ]
    | _ -> failwith "Unexpected effect"

[<Test>]
let ``take adds request item to inventory`` () =
    let shelfWithItems =
        { shelf with
            Properties = [ Storage(Shelf, placeableItems) ] }

    let effect =
        Items.take dummyState placeableItem shelfWithItems
        |> Result.unwrap
        |> List.item 1

    match effect with
    | ItemAddedToInventory(_, item) -> item |> should equal placeableItem
    | _ -> failwith "Unexpected effect"

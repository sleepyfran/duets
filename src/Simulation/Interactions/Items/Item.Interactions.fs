[<RequireQualifiedAccess>]
module Simulation.Interactions.Items

open Entities
open Simulation

/// Defines all the types of actions that can be performed on an item.
type ItemAction = Drink

/// Defines an error that happened while trying to consume an item.
type ConsumeError = ActionNotPossible

let private removeFromGameWorld state item =
    let currentPos =
        Queries.World.Common.currentWorldCoordinates state

    let location =
        Queries.Items.itemLocation state currentPos item

    match location with
    | ItemLocation.World -> [ ItemRemovedFromWorld(currentPos, item) ]
    | ItemLocation.Inventory -> [ ItemRemovedFromInventory item ]
    | ItemLocation.Nowhere ->
        [] (* This technically shouldn't happen, but let's just not remove the item. *)

/// Attempts to perform the given action on the item, if not possible (for example,
/// drinking food) it returns ActionNotPossible, otherwise returns the effects
/// that happened after consuming the item and removes it from the inventory.
let consume state (item: Item) action =
    match action with
    | Drink ->
        match item.Type with
        | ItemType.Drink drink -> Drink.drink state drink |> Ok
        | _ -> Error ActionNotPossible
    |> Result.map ((@) (removeFromGameWorld state item))

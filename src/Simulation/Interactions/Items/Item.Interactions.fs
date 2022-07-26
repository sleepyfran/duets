[<RequireQualifiedAccess>]
module Simulation.Interactions.Items

open Entities

/// Defines all the types of actions that can be performed on an item.
type ItemAction = Drink

/// Defines an error that happened while trying to consume an item.
type ConsumeError = ActionNotPossible

/// Attempts to perform the given action on the item, if not possible (for example,
/// drinking food) it returns ActionNotPossible, otherwise returns the effects
/// that happened after consuming the item and removes it from the inventory.
let consume state (item: Item) action =
    match action with
    | ItemAction.Drink ->
        match item.Type with
        | ItemType.Drink drink -> Drink.drink state drink |> Ok
        | _ -> Error ActionNotPossible
    |> Result.map ((@) [ InventoryItemRemoved item ])

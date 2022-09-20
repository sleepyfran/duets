[<RequireQualifiedAccess>]
module Simulation.Interactions.Items

open Entities
open Simulation

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

let consume state (item: Item) action =
    match action with
    | ConsumableItemInteraction.Drink ->
        match item.Type with
        | Consumable (ConsumableItemType.Drink drink) ->
            Drink.drink state drink |> Ok
        | _ -> Error ActionNotPossible
    | ConsumableItemInteraction.Eat ->
        match item.Type with
        | Consumable (ConsumableItemType.Food food) -> Food.eat state food |> Ok
        | _ -> Error ActionNotPossible
    |> Result.map ((@) (removeFromGameWorld state item))

let interact state (item: Item) action =
    let character =
        Queries.Characters.playableCharacter state

    match action with
    | InteractiveItemInteraction.Sleep when
        item.Type = (FurnitureItemType.Bed
                     |> InteractiveItemType.Furniture
                     |> Interactive)
        ->
        [ Character.Attribute.add character CharacterAttribute.Energy 80
          Character.Attribute.add character CharacterAttribute.Health 16 ]
        @ Time.AdvanceTime.advanceDayMoment' state 2
        |> Ok
    | InteractiveItemInteraction.Play when
        item.Type = (ElectronicsItemType.GameConsole
                     |> InteractiveItemType.Electronics
                     |> Interactive)
        ->
        [ Character.Attribute.add character CharacterAttribute.Mood 6 ]
        @ Time.AdvanceTime.advanceDayMoment' state 1
        |> Ok
    | InteractiveItemInteraction.Watch when
        item.Type = (ElectronicsItemType.TV
                     |> InteractiveItemType.Electronics
                     |> Interactive)
        ->
        [ Character.Attribute.add character CharacterAttribute.Mood 5 ]
        @ Time.AdvanceTime.advanceDayMoment' state 1
        |> Ok
    | _ -> Error ActionNotPossible

/// Attempts to perform the given action on the item, if not possible (for example,
/// drinking food) it returns ActionNotPossible, otherwise returns the effects
/// that happened after consuming the item and  removes it from the inventory
/// or the game world in case of consumable items like food or drink.
let perform state (item: Item) action =
    match action with
    | ItemInteraction.Consumable interaction -> consume state item interaction
    | ItemInteraction.Interactive interaction -> interact state item interaction

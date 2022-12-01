namespace Entities

[<AutoOpen>]
module ItemTypes =
    /// Defines where an item is placed in the game.
    [<RequireQualifiedAccess>]
    type ItemLocation =
        | Inventory
        | World
        | Nowhere

    (* --------------- CONSUMABLES --------------- *)

    /// Defines all types of drinks available in the game.
    type DrinkItemType =
        | Beer of amount: int<milliliter> * alcoholContent: float
        | Cola of amount: int<milliliter>

    /// Defines all types of food available in the game.
    type FoodItemType =
        | Burger of amount: int<gram>
        | Chips of amount: int<gram>
        | Fries of amount: int<gram>
        | Nachos of amount: int<gram>

    (* --------------- INTERACTIVE --------------- *)

    /// Defines all kind of electronics available in the game.
    type ElectronicsItemType =
        | TV
        | GameConsole

    /// Defines all kinds of furniture available in the game.
    type FurnitureItemType = | Bed

    /// Defines all types of items that can be consumed in the game, categorized
    /// by its kind.
    type ConsumableItemType =
        | Drink of DrinkItemType
        | Food of FoodItemType

    /// Defines all types of items that can be only interacted with and not
    /// consumed, categorized by its kind.
    type InteractiveItemType =
        | Electronics of ElectronicsItemType
        | Furniture of FurnitureItemType

    /// Defines all types of items available in the game, categorized by whether
    /// they can be consumed or only interacted with.
    type ItemType =
        | Consumable of ConsumableItemType
        | Interactive of InteractiveItemType

    /// Defines an item of the game that can be consumed by the player.
    type Item = { Brand: string; Type: ItemType }

    /// Defines an item that can be purchased, with the item itself and its price.
    type PurchasableItem = Item * Amount

    /// Defines the inventory of the character.
    type Inventory = Item list

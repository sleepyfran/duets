namespace Entities

[<AutoOpen>]
module ItemTypes =
    /// Defines all types of drinks available in the game.
    type DrinkItemType =
        | Beer of amount: int<milliliter> * alcoholContent: float
        | Cola of amount: int<milliliter>

    /// Defines all types of food available in the game.
    type FoodItemType =
        | Fries of amount: int<gram>
        | Nachos of amount: int<gram>

    /// Defines all types of items available in the game, categorized by its kind.
    type ItemType =
        | Drink of DrinkItemType
        | Food of FoodItemType

    /// Defines an item of the game that can be consumed by the player.
    type Item = { Brand: string; Type: ItemType }

    /// Defines an item that can be purchased, with the item itself and its price.
    type PurchasableItem = Item * Amount

    /// Defines the inventory of the character.
    type Inventory = Item list

﻿namespace Duets.Entities

[<AutoOpen>]
module ItemTypes =
    /// Defines where an item is placed in the game.
    [<RequireQualifiedAccess>]
    type ItemLocation =
        | Inventory
        | World
        | Nowhere

    /// Defines all types of food available in the game.
    type FoodType =
        | Unhealthy
        | Regular
        | Healthy

    /// Defines an item that can be eaten by the player.
    type EdibleItem =
        { Amount: int<gram>
          FoodType: FoodType }

    /// Defines all types of drinks available in the game.
    type DrinkType =
        | Beer of alcoholContent: float
        | Coffee of caffeineAmount: int<milliliter>
        | Soda

    /// Defines an item that can be drank by the player.
    type DrinkableItem =
        { Amount: int<milliliter>
          DrinkType: DrinkType }

    /// Defines all types of games available inside the game.
    type GameType =
        | Darts
        | Billiard
        | VideoGame

    /// Defines all kind of keys that can be used to unlock a specific place.
    type KeyItem = Chip of cityId: CityId * placeId: PlaceId

    /// Defines all types of readable items available in the game.
    type ReadableItem = Book of Book

    /// Defines all types of properties that an item can have. These properties
    /// define how an item can be used by the character and can be combined
    /// together.
    type ItemProperty =
        /// Example: a stove.
        | Cookware
        /// Example: a beer.
        | Drinkable of DrinkableItem
        /// Example: a burger.
        | Edible of EdibleItem
        /// Example: a weight machine.
        | FitnessEquipment
        /// Example: a chip to enter a place.
        | Key of KeyItem
        /// Example: a game console.
        | Playable of GameType
        /// Example: a book.
        | Readable of ReadableItem
        /// Example: a bed.
        | Sleepable
        /// Example: a TV.
        | Watchable

    /// Defines an item of the game that can be consumed by the player.
    type Item =
        {
            Brand: string
            /// Defines which properties the item has. They're ordered by importance,
            /// meaning the first one is the "main" property which will determine
            /// which kind of item this is and the rest are secondary properties
            /// that can enhance what the item can do.
            Properties: ItemProperty list
        }

    /// Defines an item that can be purchased, with the item itself and its price.
    type PurchasableItem = Item * Amount

    /// Defines the inventory of the character.
    type Inventory = Item list

    /// Contains all the items that a specific location has.
    type WorldItems = Map<RoomCoordinates, Item list>

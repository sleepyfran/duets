namespace Duets.Entities

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
        | Coffee of amount: int<milliliter>
        | Soda of amount: int<milliliter>

    /// Defines all types of food available in the game.
    type FoodItemType =
        | Unhealthy of amount: int<gram>
        | Regular of amount: int<gram>
        | Healthy of amount: int<gram>

    (* --------------- INTERACTIVE --------------- *)

    /// Defines all kind of electronics available in the game.
    type ElectronicsItemType =
        | TV
        | GameConsole
        | Dartboard

    /// Defines all kinds of furniture available in the game.
    type FurnitureItemType =
        | Bed
        | BilliardTable
        | Stove

    /// Defines all kinds of gym equipment available in the game.
    type GymEquipmentItemType =
        | WeightMachine
        | Treadmill

    (* --------------- KEYS --------------- *)

    /// Defines all kind of keys that can be used to unlock a specific place.
    type KeyItemType = Chip of cityId: CityId * placeId: PlaceId

    (* --------------- AGGREGATED ITEMS --------------- *)

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
        | GymEquipment of GymEquipmentItemType

    /// Defines all types of items available in the game, categorized by whether
    /// they can be consumed or only interacted with.
    type ItemType =
        | Consumable of ConsumableItemType
        | Interactive of InteractiveItemType
        | Key of KeyItemType

    /// Defines an item of the game that can be consumed by the player.
    type Item = { Brand: string; Type: ItemType }

    /// Defines an item that can be purchased, with the item itself and its price.
    type PurchasableItem = Item * Amount

    /// Defines the inventory of the character.
    type Inventory = Item list

    /// Contains all the items that a specific location has.
    type WorldItems = Map<RoomCoordinates, Item list>

module rec Duets.Simulation.Items

open Duets.Common
open Duets.Entities
open Duets.Simulation

type ItemError =
    | ItemIsNotPlaceable
    | StorageItemIsNotStorage

/// Attempts to place an item in a given shelf. If the item is not placeable in
/// the shelf, an error is returned. Likewise, if the shelf is not a storage
/// or the storage type does not match the required one by the item, it returns
/// an error.
let place state item shelf =
    let placeableType =
        Item.Property.tryPick
            (function
            | Storage(storage, _) -> Some storage
            | _ -> None)
            shelf

    let itemIsPlaceable storageType =
        Item.Property.has
            (function
            | PlaceableInStorage storage -> storage = storageType
            | _ -> false)
            item

    match placeableType with
    | Some placeableType when itemIsPlaceable placeableType ->
        let coords = Queries.World.currentCoordinates state

        let updatedShelf =
            { shelf with
                Properties =
                    shelf.Properties
                    |> List.map (function
                        | Storage(storageType, items) ->
                            Storage(storageType, item :: items)
                        | p -> p) }

        [ (coords, Diff(shelf, updatedShelf)) |> ItemChangedInWorld
          ItemRemovedFromCharacterInventory item ]
        |> Ok
    | Some _ -> Error ItemIsNotPlaceable
    | None -> Error StorageItemIsNotStorage

/// Returns the contents of a storage item. Otherwise returns an error.
let from shelf =
    let items =
        Item.Property.tryPick
            (function
            | Storage(_, items) -> Some items
            | _ -> None)
            shelf

    Result.ofOption items StorageItemIsNotStorage

/// Attempts to take the given object from the storage item. Otherwise returns
/// an error if the given shelf does not have a storage property.
let take state item storage =
    let itemIsStorage =
        Item.Property.has
            (function
            | Storage _ -> true
            | _ -> false)
            storage

    if itemIsStorage then
        let coords = Queries.World.currentCoordinates state

        let updatedShelf =
            { storage with
                Properties =
                    storage.Properties
                    |> List.map (function
                        | Storage(storageType, items) ->
                            Storage(storageType, List.filter ((<>) item) items)
                        | p -> p) }

        [ (coords, Diff(storage, updatedShelf)) |> ItemChangedInWorld
          ItemAddedToCharacterInventory item ]
        |> Ok
    else
        Error StorageItemIsNotStorage

/// Removes an item from either the world or the player's inventory.
let remove state item =
    let coords = Queries.World.currentCoordinates state
    let location = Queries.Items.itemLocation state coords item

    match location with
    | ItemLocation.World -> [ ItemRemovedFromWorld(coords, item) ]
    | ItemLocation.Inventory ->
        [ ItemRemovedFromCharacterInventory item ]
    | ItemLocation.Nowhere -> []

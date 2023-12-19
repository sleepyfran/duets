module rec Duets.Simulation.Items

open Duets.Entities
open Duets.Simulation

type PlaceItemError =
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
          ItemRemovedFromInventory item ]
        |> Ok
    | Some _ -> Error ItemIsNotPlaceable
    | None -> Error StorageItemIsNotStorage

/// Removes an item from either the world or the player's inventory.
let remove state item =
    let coords = Queries.World.currentCoordinates state
    let location = Queries.Items.itemLocation state coords item

    match location with
    | ItemLocation.World -> [ ItemRemovedFromWorld(coords, item) ]
    | ItemLocation.Inventory -> [ ItemRemovedFromInventory item ]
    | ItemLocation.Nowhere -> []

namespace Simulation.Queries

open Aether
open Common
open Entities

module Items =
    /// Returns all the items currently available in the given coordinates.
    let allIn state coords =
        Optic.get Lenses.State.worldItems_ state
        |> Map.tryFind coords
        |> Option.defaultValue []

    /// Attempts to find an item in the given world position or the character's
    /// inventory by name.
    let findByName state locationCoords name =
        allIn state locationCoords @ Inventory.get state
        |> List.tryFind (fun item ->
            String.diacriticInsensitiveContains name item.Brand)

    /// Determines whether the given item is located in the given world location,
    /// the character's inventory or none of them.
    let itemLocation state location item =
        let locationItems = allIn state location
        let inventory = Inventory.get state

        if locationItems |> List.contains item then
            ItemLocation.World
        else if inventory |> List.contains item then
            ItemLocation.Inventory
        else
            ItemLocation.Nowhere

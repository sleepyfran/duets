namespace Simulation.Queries

open Aether
open Common
open Entities

module Inventory =
    /// Returns the content of the character's inventory.
    let get =
        Optic.get Lenses.State.characterInventory_

    /// Attempts to find an item in the inventory by a given name.
    let findByName state name =
        get state
        |> List.tryFind (fun item ->
            String.diacriticInsensitiveContains name item.Brand)

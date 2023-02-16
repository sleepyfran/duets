namespace Duets.Simulation.Queries

open Aether
open Duets.Entities

module Inventory =
    /// Returns the content of the character's inventory.
    let get =
        Optic.get Lenses.State.characterInventory_

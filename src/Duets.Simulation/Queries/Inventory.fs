namespace Duets.Simulation.Queries

open Aether
open Aether.Operators
open Duets.Common
open Duets.Entities

module Inventory =
    /// Returns the content of the character's inventory.
    let character =
        Optic.get (Lenses.State.inventories_ >-> Lenses.Inventories.character_)

    /// Returns the content of a specific band's inventory.
    let band id state =
        Optic.get
            (Lenses.State.inventories_
             >-> Lenses.Inventories.band_
             >-> Map.keyWithDefault_ id Map.empty)
            state
        |> Option.defaultValue Map.empty

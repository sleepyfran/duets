module Duets.Simulation.State.Inventory

open Aether
open Duets.Common
open Duets.Entities

let add item =
    Optic.map Lenses.State.characterInventory_ (List.append [ item ])

let remove item =
    Optic.map
        Lenses.State.characterInventory_
        (List.removeFirstOccurrenceOf item)

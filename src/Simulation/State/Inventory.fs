module Simulation.State.Inventory

open Aether
open Common
open Entities

let add item =
    Optic.map Lenses.State.characterInventory_ (List.append [ item ])

let remove item =
    Optic.map
        Lenses.State.characterInventory_
        (List.removeFirstOccurrenceOf item)

module Duets.Simulation.State.Inventory

open Aether
open Aether.Operators
open Duets.Common
open Duets.Entities

let private charactersLenses =
    Lenses.State.inventories_ >-> Lenses.Inventories.character_

let private bandsLenses = Lenses.State.inventories_ >-> Lenses.Inventories.band_

let private add item lens = Optic.map lens (List.append [ item ])

let private remove item lens =
    Optic.map lens (List.removeFirstOccurrenceOf item)

let private lensFromKey key =
    match key with
    | InventoryKey.Character -> charactersLenses
    | InventoryKey.Band -> bandsLenses

let addTo key item = lensFromKey key |> add item

let removeFrom key item = lensFromKey key |> remove item

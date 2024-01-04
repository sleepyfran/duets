module Duets.Simulation.State.Inventory

open Aether
open Aether.Operators
open Duets.Common
open Duets.Entities

let private charactersLenses =
    Lenses.State.inventories_ >-> Lenses.Inventories.character_

let private bandsLenses = Lenses.State.inventories_ >-> Lenses.Inventories.band_

let addToCharacter item =
    Optic.map charactersLenses (List.append [ item ])

let removeFromCharacter item =
    Optic.map charactersLenses (List.removeFirstOccurrenceOf item)

let addToBand item quantity =
    // TODO: Check what happens here when the item is already in the map, does it overwrite it?
    Optic.map bandsLenses (Map.add item quantity)

let removeFromBand item = Optic.map bandsLenses (Map.remove item)

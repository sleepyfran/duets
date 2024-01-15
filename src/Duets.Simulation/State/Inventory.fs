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
    Optic.map
        bandsLenses
        (Map.change item (fun q ->
            match q with
            | Some q -> Some(q + quantity)
            | None -> Some quantity))

let reduceForBand item quantity =
    Optic.map
        bandsLenses
        (Map.change item (function
            | Some q -> Some(q - quantity)
            | None -> None))

let removeFromBand item = Optic.map bandsLenses (Map.remove item)

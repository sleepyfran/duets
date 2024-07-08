module Duets.Simulation.State.Inventory

open Aether
open Aether.Operators
open Duets.Common
open Duets.Entities

let private charactersLenses =
    Lenses.State.inventories_ >-> Lenses.Inventories.character_

let private bandsLenses id =
    Lenses.State.inventories_
    >-> Lenses.Inventories.band_
    >-> Map.keyWithDefault_ id Map.empty

let addToCharacter item =
    Optic.map charactersLenses (List.append [ item ])

let removeFromCharacter item =
    Optic.map charactersLenses (List.removeFirstOccurrenceOf item)

let addToBand bandId item quantity =
    Optic.map
        (bandsLenses bandId)
        (Map.change item (fun q ->
            match q with
            | Some q -> Some(q + quantity)
            | None -> Some quantity))

let reduceForBand bandId item quantity =
    Optic.map
        (bandsLenses bandId)
        (Map.change item (function
            | Some q -> Some(q - quantity)
            | None -> None))

let removeFromBand bandId item =
    Optic.map (bandsLenses bandId) (Map.remove item)

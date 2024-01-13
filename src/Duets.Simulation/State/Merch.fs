module Duets.Simulation.State.Merch

open Aether
open Aether.Operators
open Duets.Common
open Duets.Entities

let setPrice bandId itemProperty price state =
    let mainItemProperty = Item.Property.tryMain itemProperty

    match mainItemProperty with
    | Some property ->
        let lens =
            Lenses.State.merchPrices_ >-> Map.keyWithDefault_ bandId Map.empty
            >?> Map.keyWithDefault_ property 0m<dd>

        Optic.set lens price state
    | None -> state

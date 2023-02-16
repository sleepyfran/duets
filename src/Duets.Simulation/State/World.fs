module Duets.Simulation.State.World

open Aether
open Duets.Entities

let move cityId placeId =
    Optic.set Lenses.State.currentPosition_ (cityId, placeId)

let remove coords item =
    let removeItem =
        Map.change coords (function
            | Some list -> List.except [ item ] list |> Some
            | None -> None)

    Optic.map Lenses.State.worldItems_ removeItem

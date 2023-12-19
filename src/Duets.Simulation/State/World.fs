module Duets.Simulation.State.World

open Aether
open Duets.Entities

let move cityId placeId roomId =
    Optic.set Lenses.State.currentPosition_ (cityId, placeId, roomId)

let add coords item =
    let addItem =
        Map.change coords (function
            | Some list -> item :: list |> Some
            | None -> Some [ item ])

    Optic.map Lenses.State.worldItems_ addItem

let remove coords item =
    let removeItem =
        Map.change coords (function
            | Some list -> List.except [ item ] list |> Some
            | None -> None)

    Optic.map Lenses.State.worldItems_ removeItem

let setPeople people =
    Optic.set Lenses.State.peopleInCurrentPosition_ people

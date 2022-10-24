module rec UI.Scenes.InGame.FreeRoam

open Entities

let runFreeRoamInteraction state interaction =
    match interaction with
    | FreeRoamInteraction.Wait -> unimplemented ()
    | _ -> failwith "Not supported"

let private unimplemented () = failwith "Oops"

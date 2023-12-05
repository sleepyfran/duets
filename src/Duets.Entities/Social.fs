module Duets.Entities.Social

open Aether

module State =
    let timesDoneAction socializingState action =
        Optic.get Lenses.SocializingState.actions_ socializingState
        |> List.filter (fun a -> a = action)
        |> List.length
        |> (*) 1<times>

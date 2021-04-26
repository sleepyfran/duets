module Data.Queries.VocalStyles

open Microsoft.FSharp.Reflection
open Entities

/// Retrieves the name of all the roles available.
let getNames () =
  FSharpType.GetUnionCases typeof<VocalStyle>
  |> Array.map (fun uc -> uc.Name)
  |> List.ofArray

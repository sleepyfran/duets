module Data.Queries.Roles

open Entities.Band
open Microsoft.FSharp.Reflection

/// Retrieves the name of all the roles available.
let getNames () =
  FSharpType.GetUnionCases typeof<Role>
  |> Array.map (fun uc -> uc.Name)
  |> List.ofArray

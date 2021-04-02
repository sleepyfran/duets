module Data.Queries.Roles

open Microsoft.FSharp.Reflection
open Entities.Band

/// Retrieves the name of all the roles available.
let getNames () =
  FSharpType.GetUnionCases typeof<Role>
  |> Array.map (fun uc -> uc.Name)
  |> List.ofArray

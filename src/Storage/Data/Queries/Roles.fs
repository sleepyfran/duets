module Data.Queries.Roles

open Microsoft.FSharp.Reflection
open Entities

/// Retrieves the name of all the roles available.
let getNames () =
  FSharpType.GetUnionCases typeof<MemberRole>
  |> Array.map (fun uc -> uc.Name)
  |> List.ofArray

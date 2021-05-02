module Data.Roles

open Microsoft.FSharp.Reflection
open Entities

/// Retrieves the name of all the roles available.
let getNames () =
  FSharpType.GetUnionCases typeof<InstrumentType>
  |> Array.map (fun uc -> uc.Name)
  |> List.ofArray

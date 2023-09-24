module Duets.Common.Union

open FSharp.Reflection

/// Returns a list with all cases of a discriminated union. This only works for
/// unions with no arguments.
let allCasesOf<'a> () : 'a list =
    FSharpType.GetUnionCases typeof<'a>
    |> Array.map (fun uc -> FSharpValue.MakeUnion(uc, [||]) :?> 'a)
    |> List.ofArray

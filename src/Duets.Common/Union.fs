module Duets.Common.Union

open FSharp.Reflection

/// Returns a list with all cases of a discriminated union. This only works for
/// unions with no arguments. For those that have arguments they'll be ignored
/// from the list.
let allCasesOf<'a> () : 'a list =
    FSharpType.GetUnionCases typeof<'a>
    |> Array.choose (fun uc ->
        let constructorArgs = uc.GetFields()

        if constructorArgs.Length > 0 then
            None
        else
            FSharpValue.MakeUnion(uc, [||]) :?> 'a |> Some)
    |> List.ofArray

/// Returns the name of one case of a discriminated union.
let caseName (x: 'a) =
    match FSharpValue.GetUnionFields(x, typeof<'a>) with
    | case, _ -> case.Name

module Duets.Data.Tests.Items

open Microsoft.FSharp.Reflection
open FsUnit
open NUnit.Framework

open Duets.Entities
open Duets.Data

[<Test>]
let ``all city IDs have at least one beer defined in the byLocation map`` () =
    FSharpType.GetUnionCases typeof<CityId>
    |> Array.map (fun uc -> FSharpValue.MakeUnion(uc, [||]) :?> CityId)
    |> List.ofArray
    |> List.forall (fun cityId ->
        Items.Drink.Beer.byLocation |> Map.containsKey cityId)
    |> should equal true

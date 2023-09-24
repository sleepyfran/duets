module Duets.Data.Tests.Items

open FsUnit
open NUnit.Framework

open Duets.Common
open Duets.Entities
open Duets.Data

[<Test>]
let ``all city IDs have at least one beer defined in the byLocation map`` () =
    Union.allCasesOf<CityId> ()
    |> List.forall (fun cityId ->
        Items.Drink.Beer.byLocation |> Map.containsKey cityId)
    |> should equal true

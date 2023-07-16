module Data.Tests

open FsUnit
open NUnit.Framework

open Duets.Common
open Duets.Entities
open Duets.Data.World

let rec private checkCities (cities: City list) =
    // Go through all cities and check that they are connected to all the
    // following cities.
    match cities with
    | city1 :: rest ->
        rest
        |> List.forall (fun city2 ->
            let connection = World.connectionBetween city1.Id city2.Id
            let distance = World.distances |> Map.find connection
            distance <> 0<km>)
        |> should equal true

        checkCities rest
    | _ -> ()

[<Test>]
let ``all cities are connected to each other`` () =
    (World.get ()).Cities |> List.ofMapValues |> checkCities

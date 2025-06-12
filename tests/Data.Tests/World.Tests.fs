module Duets.Data.Tests.World

open FsUnit
open NUnit.Framework

open Duets.Common
open Duets.Entities
open Duets.Data.World
open Duets.Simulation

let rec private checkCities (cities: City list) =
    // Go through all cities and check that they are connected to all the
    // following cities.
    match cities with
    | city1 :: rest ->
        rest
        |> List.forall (fun city2 ->
            try
                let distance = World.distanceBetween city1.Id city2.Id
                distance <> 0<km>
            with _ ->
                printf $"No connection between {city1.Id} and {city2.Id}"
                false)
        |> should equal true

        checkCities rest
    | _ -> ()

[<Test>]
let ``all city IDs are added to the world`` () =
    Union.allCasesOf<CityId> ()
    |> List.forall (fun city -> World.get.Cities |> Map.containsKey city)
    |> should equal true

[<Test>]
let ``all cities are connected to each other`` () =
    World.get.Cities |> List.ofMapValues |> checkCities

[<Test>]
let ``all cities have a country`` () =
    World.get.Cities
    |> List.ofMapValues
    |> List.iter (fun city ->
        (fun () -> World.countryOf city.Id |> ignore)
        |> should
            not'
            (throw typeof<System.Collections.Generic.KeyNotFoundException>))

let private checkAtLeastOneWithCapacity
    cityId
    concertSpaces
    minCapacity
    maxCapacity
    =
    let numberOfSpaces =
        concertSpaces
        |> List.filter (function
            | capacity when capacity >= minCapacity && capacity <= maxCapacity ->
                true
            | _ -> false)
        |> List.length

    if numberOfSpaces < 1 then
        failwith
            $"{cityId} does not contain any space with capacity between {minCapacity} and {maxCapacity}"

let private checkConcertSpaces (city: City) =
    let concertSpaces =
        Queries.World.placesByTypeInCity city.Id PlaceTypeIndex.ConcertSpace
        |> List.map (fun place ->
            match place.PlaceType with
            | PlaceType.ConcertSpace concertSpace -> concertSpace.Capacity
            | _ -> 0)

    checkAtLeastOneWithCapacity city.Id concertSpaces 0 300
    checkAtLeastOneWithCapacity city.Id concertSpaces 300 500
    checkAtLeastOneWithCapacity city.Id concertSpaces 500 5000
    checkAtLeastOneWithCapacity city.Id concertSpaces 500 20000
    checkAtLeastOneWithCapacity city.Id concertSpaces 500 500000

[<Test>]
let ``all cities must have concert spaces to accomodate all sort of bands by capacity``
    ()
    =
    World.get.Cities |> List.ofMapValues |> List.iter checkConcertSpaces

[<Test>]
let ``all cities must have a hospital`` () =
    World.get.Cities
    |> List.ofMapValues
    |> List.iter (fun city ->
        let hospitals =
            Queries.World.placesByTypeInCity city.Id PlaceTypeIndex.Hospital

        System.Console.WriteLine city.Id
        hospitals |> should haveLength 1)

[<Test>]
let ``all cities must have an airport`` () =
    World.get.Cities
    |> List.ofMapValues
    |> List.iter (fun city ->
        let hospitals =
            Queries.World.placesByTypeInCity city.Id PlaceTypeIndex.Airport

        System.Console.WriteLine city.Id
        hospitals |> should haveLength 1)

// TODO: Add tests for traversing the cities and checking for crashes!

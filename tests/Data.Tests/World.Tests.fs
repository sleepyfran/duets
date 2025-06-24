module Duets.Data.Tests.World

open FsUnit
open NUnit.Framework
open Test.Common.Generators

open Duets.Common
open Duets.Entities
open Duets.Data.World
open Duets.Simulation
open Duets.Simulation.Navigation

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
    |> List.iter (fun city ->
        let cityAdded = World.get.Cities |> Map.containsKey city

        if not cityAdded then
            failwith $"{city} has not been added to the world")

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

        if hospitals.Length = 0 then
            failwith $"{city.Id} does not have a hospital")

[<Test>]
let ``all cities must have hotels`` () =
    World.get.Cities
    |> List.ofMapValues
    |> List.iter (fun city ->
        let hotels =
            Queries.World.placesByTypeInCity city.Id PlaceTypeIndex.Hotel

        if hotels.Length <= 1 then
            failwith $"{city.Id} does not have enough hotels")

[<Test>]
let ``all cities must have a home`` () =
    World.get.Cities
    |> List.ofMapValues
    |> List.iter (fun city ->
        let homes =
            Queries.World.placesByTypeInCity city.Id PlaceTypeIndex.Home

        if homes.Length = 0 then
            failwith $"{city.Id} does not have a home")

[<Test>]
let ``all cities must have an airport`` () =
    World.get.Cities
    |> List.ofMapValues
    |> List.iter (fun city ->
        let airports =
            Queries.World.placesByTypeInCity city.Id PlaceTypeIndex.Airport

        if airports.Length = 0 then
            failwith $"{city.Id} does not have an airport")

[<Test>]
let ``player is able to exit every place in the world`` () =
    let state =
        State.generateOne
            { State.defaultOptions with
                Today =
                    Calendar.Date.fromSeasonAndYear Winter 2025<years>
                    |> Calendar.Transform.changeDayMoment Midday }

    World.get.Cities
    |> List.ofMapValues
    |> List.iter (fun city ->
        city.PlaceIndex
        |> Map.iter (fun placeId _ ->
            let place = Queries.World.placeInCityById city.Id placeId
            let startingRoom = place.Rooms.StartingNode

            place.Exits
            |> Map.iter (fun _ streetId ->
                let state =
                    { state with
                        CurrentPosition = (city.Id, place.Id, startingRoom) }

                (fun () -> Navigation.exitTo streetId state |> ignore)
                |> should not' (throw typeof<System.Exception>))))

[<Test>]
let ``all metro stations in each city are reachable`` () =
    // For each city, ensure that every metro station belongs to a connected
    // component, i.e. you can reach every other station by travelling through
    // successive `MetroStationConnection`s.
    let buildAdjacency (metroLines: MetroLine list) =
        metroLines
        |> List.collect (fun line ->
            line.Stations
            |> Map.toList
            |> List.collect (fun (zoneId, connection) ->
                match connection with
                | OnlyNext next -> [ zoneId, next; next, zoneId ]
                | OnlyPrevious prev -> [ zoneId, prev; prev, zoneId ]
                | PreviousAndNext(prev, next) ->
                    [ zoneId, prev; prev, zoneId; zoneId, next; next, zoneId ]))
        |> List.groupBy fst
        |> List.map (fun (from, tos) -> from, tos |> List.map snd |> Set.ofList)
        |> Map.ofList

    let rec bfs (adj: Map<ZoneId, Set<ZoneId>>) queue visited =
        match queue with
        | [] -> visited
        | current :: rest ->
            let neighbours =
                adj |> Map.tryFind current |> Option.defaultValue Set.empty

            let unseen =
                neighbours
                |> Set.filter (fun n -> not (visited |> Set.contains n))

            bfs adj (rest @ (unseen |> Set.toList)) (Set.union visited unseen)

    World.get.Cities
    |> List.ofMapValues
    |> List.iter (fun city ->
        let metroLines = city.MetroLines |> List.ofMapValues

        let stationIds =
            metroLines
            |> List.collect (fun line ->
                line.Stations |> Map.toList |> List.map fst)
            |> Set.ofList

        if stationIds.Count > 0 then
            let adjacency = buildAdjacency metroLines
            let start = stationIds |> Seq.head
            let visited = bfs adjacency [ start ] (Set.singleton start)

            if visited.Count <> stationIds.Count then
                failwith $"""{city.Id} has unreachable metro stations""")

[<Test>]
let ``player can moveTo every metro station in a city`` () =
    let state = State.generateOne State.defaultOptions

    World.get.Cities
    |> List.ofMapValues
    |> List.iter (fun city ->
        let stationPlaceIds =
            city.Zones
            |> List.ofMapValues
            |> List.collect (fun zone ->
                zone.MetroStations |> List.ofMapValues)
            |> List.map _.PlaceId
            |> Set.ofList
            |> Set.toList

        match stationPlaceIds with
        | [] -> ()
        | startingPlaceId :: _ ->
            let startingPlace =
                Queries.World.placeInCityById city.Id startingPlaceId

            let startingRoom = startingPlace.Rooms.StartingNode

            stationPlaceIds
            |> List.iter (fun targetPlaceId ->
                let state =
                    { state with
                        CurrentPosition =
                            (city.Id, startingPlaceId, startingRoom) }

                try
                    Navigation.moveTo targetPlaceId state |> ignore
                with ex ->
                    let decodedTarget =
                        Identity.Reproducible.decode targetPlaceId

                    failwith
                        $"There was an exception thrown when moving from {startingPlace.Name} towards {decodedTarget} by metro in {city.Id}\nException: {ex.Message}"))

[<Test>]
let ``every zone has at least another one metro station`` () =
    World.get.Cities
    |> List.ofMapValues
    |> List.iter (fun city ->
        let zones = city.Zones

        zones
        |> List.ofMapValues
        |> List.iter (fun zone ->
            let metroStations = zone.MetroStations

            if metroStations.Count = 0 then
                let decodedZone = Identity.Reproducible.decode zone.Id

                failwith
                    $"{city.Id} has a zone ({decodedZone}) with no metro stations"))

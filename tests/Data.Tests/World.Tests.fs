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
let ``all cities must have at least one place of each type`` () =
    let citiesWithMissingPlaces =
        Queries.World.allCities
        |> List.map (fun city ->
            Union.allCasesOf<PlaceTypeIndex> ()
            |> List.choose (fun placeType ->
                let places =
                    Queries.World.placesByTypeInCity city.Id placeType

                if places.Length = 0 then Some(city.Id, placeType) else None))
        |> List.concat

    if citiesWithMissingPlaces.Length > 0 then
        let errorMessage =
            citiesWithMissingPlaces
            |> List.groupBy fst
            |> List.map (fun (cityId, places) ->
                let missingPlaceTypes =
                    places
                    |> List.map snd
                    |> List.map string
                    |> String.concat ", "

                $"{cityId}: {missingPlaceTypes}")
            |> String.concat "\n"

        failwith
            $"The following cities are missing place types:\n{errorMessage}"

[<Test>]
let ``all non-street places in the world must have at least one exit`` () =
    let placesWithoutExits =
        World.get.Cities
        |> List.ofMapValues
        |> List.collect (fun city ->
            city.PlaceIndex
            |> List.ofMap
            |> List.choose (fun (placeId, _) ->
                let place = Queries.World.placeInCityById city.Id placeId

                match place.PlaceType with
                | Street -> None
                | _ ->
                    let exists = place.Exits |> Map.count
                    if exists <= 0 then Some(place, city.Id) else None))

    if placesWithoutExits.Length > 0 then
        let formattedPlaces =
            placesWithoutExits
            |> List.map (fun (place, cityId) ->
                let zone = Queries.World.zoneInCityById cityId place.ZoneId

                $"{place.Name} in {cityId} ({zone.Name}) does not have any exit")
            |> String.concat "\n"

        failwith formattedPlaces

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

[<Test>]
let ``there should not be two places with the same ID`` () =
    let mutable duplicated: (CityId * PlaceId * string) list = List.empty

    World.get.Cities
    |> List.ofMapValues
    |> List.iter (fun city ->
        let mutable visited: Set<PlaceId> = Set.empty

        city.PlaceByTypeIndex
        |> List.ofMapValues
        |> List.concat
        |> List.iter (fun place ->
            if Set.contains place visited then
                let decoded = Identity.Reproducible.decode place
                duplicated <- (city.Id, place, decoded) :: duplicated
            else
                visited <- Set.add place visited))

    if duplicated.Length > 0 then
        duplicated
        |> List.map (fun (city, _, name) -> $"{name} in {city} is duplicated")
        |> String.concat "\n"
        |> failwith

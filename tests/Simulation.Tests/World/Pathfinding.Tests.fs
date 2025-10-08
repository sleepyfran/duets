module Duets.Simulation.Tests.World.Pathfinding

open FsUnit
open NUnit.Framework

open Duets.Common
open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Navigation

[<Test>]
let ``directionsToNode returns None when places are in different cities`` () =
    let placeInLondon =
        Queries.World.placesByTypeInCity London PlaceTypeIndex.Hotel
        |> List.head

    let placeInPrague =
        Queries.World.placesByTypeInCity Prague PlaceTypeIndex.Hotel
        |> List.head

    let result =
        Pathfinding.directionsToNode London placeInLondon.Id placeInPrague.Id

    result |> should equal None

[<Test>]
let ``directionsToNode returns directions when both places are on the same street``
    ()
    =
    let pretAManger =
        Identity.Reproducible.create "Pret a Manger"
        |> Queries.World.placeInCityById London

    let cafeNero =
        Identity.Reproducible.create "Cafe Nero"
        |> Queries.World.placeInCityById London

    let result = Pathfinding.directionsToNode London pretAManger.Id cafeNero.Id

    match result with
    | Some directions ->
        directions.Length |> should equal 2

        match directions with
        | [ Pathfinding.GoOut(fromPlace, toStreet)
            Pathfinding.Enter(street, targetPlace) ] ->
            fromPlace.Id |> should equal pretAManger.Id
            targetPlace.Id |> should equal cafeNero.Id
            toStreet.Id |> should equal street.Id
        | _ -> failwith "Directions do not match expected"
    | None ->
        failwith "Expected to find a path between places on the same street"

[<Test>]
let ``directionsToNode returns directions when places are in the same zone but different streets``
    ()
    =
    let hotelEmperador =
        Identity.Reproducible.create "Hotel Emperador"
        |> Queries.World.placeInCityById Madrid

    let casaLabra =
        Identity.Reproducible.create "Casa Labra"
        |> Queries.World.placeInCityById Madrid

    let result =
        Pathfinding.directionsToNode Madrid hotelEmperador.Id casaLabra.Id

    match result with
    | Some [ Pathfinding.GoOut(fromPlace, firstStreet)
             Pathfinding.Walk(_, secondStreet, direction)
             Pathfinding.Enter(fromStreet, toPlace) ] ->
        fromPlace.Id |> should equal hotelEmperador.Id
        firstStreet.Name |> should equal "Gran Vía"
        secondStreet.Name |> should equal "Puerta del Sol"
        fromStreet.Name |> should equal secondStreet.Name
        direction |> should equal South
        toPlace.Id |> should equal casaLabra.Id
    | None ->
        failwith
            $"Expected to find path between {hotelEmperador.Id} and {casaLabra.Id}"
    | _ -> failwith "Path did not match what was expected"

[<Test>]
let ``directionsToNode navigates through multiple streets in the same zone``
    ()
    =
    let hotelEmperador =
        Identity.Reproducible.create "Hotel Emperador"
        |> Queries.World.placeInCityById Madrid

    let estudioUno =
        Identity.Reproducible.create "Estudio Uno"
        |> Queries.World.placeInCityById Madrid

    let result =
        Pathfinding.directionsToNode Madrid hotelEmperador.Id estudioUno.Id

    match result with
    | Some [ Pathfinding.GoOut(exitFromPlace, exitToFirstStreet)
             Pathfinding.Walk(_, secondStreet, firstWalkDirection)
             Pathfinding.Walk(_, thirdStreet, secondWalkDirection)
             Pathfinding.Enter(enterFromStreet, enterToPlace) ] ->
        exitFromPlace.Id |> should equal hotelEmperador.Id
        exitToFirstStreet.Name |> should equal "Gran Vía"
        secondStreet.Name |> should equal "Puerta del Sol"
        firstWalkDirection |> should equal South

        thirdStreet.Name |> should equal "Barrio de las Letras"
        secondWalkDirection |> should equal West
        enterFromStreet.Name |> should equal thirdStreet.Name

        enterToPlace.Id |> should equal estudioUno.Id
    | None ->
        failwith
            $"Expected to find path between {hotelEmperador.Id} and {estudioUno.Id}"
    | _ -> failwith "Path did not match what was expected"

[<Test>]
let ``directionsToNode navigates through metro to connect different zones`` () =
    let dreamcatcherInn =
        Identity.Reproducible.create "The Dreamcatcher Inn"
        |> Queries.World.placeInCityById LosAngeles

    let kTownBroadcastTower =
        Identity.Reproducible.create "K-Town Broadcast Tower"
        |> Queries.World.placeInCityById LosAngeles

    let result =
        Pathfinding.directionsToNode
            LosAngeles
            dreamcatcherInn.Id
            kTownBroadcastTower.Id

    match result with
    | Some directions ->
        directions.Length |> should be (greaterThan 0)

        // First should be GoOut from The Dreamcatcher Inn.
        match List.head directions with
        | Pathfinding.GoOut(fromPlace, _) ->
            fromPlace.Id |> should equal dreamcatcherInn.Id
        | _ -> failwith "Expected first direction to be GoOut"

        // Last should be Enter to K-Town Broadcast Tower.
        match List.last directions with
        | Pathfinding.Enter(_, targetPlace) ->
            targetPlace.Id |> should equal kTownBroadcastTower.Id
        | _ -> failwith "Expected last direction to be Enter"

        // Should contain TakeMetro directions since zones are different.
        let metroDirections =
            directions
            |> List.filter (function
                | Pathfinding.TakeMetro _ -> true
                | _ -> false)

        metroDirections.Length |> should be (greaterThan 0)

        // Should use Red line (Hollywood -> Downtown LA) and Blue line (Downtown LA -> Koreatown).
        let linesUsed =
            metroDirections
            |> List.map (function
                | Pathfinding.TakeMetro(_, _, lineId) -> lineId
                | _ -> Red)
            |> Set.ofList

        linesUsed |> should contain Red
        linesUsed |> should contain Blue
    | None ->
        failwith
            "Expected to find a path between Hollywood and Koreatown via metro"

[<Test>]
let ``directionsToNode navigates through multiple metro transfers`` () =
    let dreamcatcherInn =
        Identity.Reproducible.create "The Dreamcatcher Inn"
        |> Queries.World.placeInCityById LosAngeles

    let laxAirport =
        Identity.Reproducible.create "Los Angeles International Airport"
        |> Queries.World.placeInCityById LosAngeles

    let result =
        Pathfinding.directionsToNode LosAngeles dreamcatcherInn.Id laxAirport.Id

    match result with
    | Some directions ->
        directions.Length |> should be (greaterThan 0)

        // First should be GoOut from The Dreamcatcher Inn.
        match List.head directions with
        | Pathfinding.GoOut(fromPlace, _) ->
            fromPlace.Id |> should equal dreamcatcherInn.Id
        | _ -> failwith "Expected first direction to be GoOut"

        // Last should be Enter to LAX Airport.
        match List.last directions with
        | Pathfinding.Enter(_, targetPlace) ->
            targetPlace.Id |> should equal laxAirport.Id
        | _ -> failwith "Expected last direction to be Enter"

        // Should contain multiple TakeMetro directions.
        let metroDirections =
            directions
            |> List.filter (function
                | Pathfinding.TakeMetro _ -> true
                | _ -> false)

        metroDirections.Length |> should be (greaterThanOrEqualTo 3)

        // Should use Red line (Hollywood -> Downtown LA) and Blue line (Downtown LA -> Koreatown -> Santa Monica -> LAX).
        let linesUsed =
            metroDirections
            |> List.map (function
                | Pathfinding.TakeMetro(_, _, lineId) -> lineId
                | _ -> Red)
            |> Set.ofList

        linesUsed |> should contain Red
        linesUsed |> should contain Blue
    | None ->
        failwith
            "Expected to find a path between Hollywood and LAX via multiple metro transfers"


[<Test>]
let ``directionsToNode handles multiple exits`` () =
    (*
    Currently there are no places in the game world with multiple exits, so this
    test is pointless, but adding it here to remind myself to deal with the comment
    if I ever add multiple exits.
    *)
    Queries.World.allCities
    |> List.iter (fun city ->
        Queries.World.allPlacesInCity city.Id
        |> List.ofMapValues
        |> List.concat
        |> List.iter (fun place ->
            if Map.count place.Exits > 1 then
                failwith
                    "directionsToNode is currently not able to deal with multiple exits properly, implement it!"))

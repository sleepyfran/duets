module Duets.Simulation.Tests.Car

open Duets.Data.Items
open Duets.Data.World
open Duets.Simulation.Vehicles
open NUnit.Framework
open FsUnit
open Test.Common.Generators

open Duets.Common
open Duets.Entities
open Duets.Simulation

let currentPlace =
    Queries.World.placesByTypeInCity Prague PlaceTypeIndex.Restaurant
    |> List.find (fun place -> place.Name = "Bistro Stromovka")

let currentStreet =
    currentPlace.Exits |> Map.head |> Queries.World.streetById Prague

let state =
    State.generateOne State.defaultOptions
    |> State.World.move Prague currentPlace.Id Ids.Common.bar

let testCar, _ = Vehicles.Car.toyotaCorolla

let carPosition = (Prague, currentStreet.Id, "0")

(* == Within city drive tests == *)

[<Test>]
let ``planWithinCityDrive returns AlreadyAtDestination when already at destination``
    ()
    =
    let currentPlace = Queries.World.currentPlace state

    let result = Car.planWithinCityDrive state currentPlace

    match result with
    | Error Car.AlreadyAtDestination -> ()
    | _ -> failwith "Expected AlreadyAtDestination error"

[<Test>]
let ``planWithinCityDrive returns CannotReachDestination when no path exists``
    ()
    =
    let placeInDifferentCity =
        Queries.World.placesByTypeInCity London PlaceTypeIndex.Hotel
        |> List.head

    let result = Car.planWithinCityDrive state placeInDifferentCity

    match result with
    | Error Car.CannotReachDestination -> ()
    | _ -> failwith "Expected CannotReachDestination error"

[<Test>]
let ``planWithinCityDrive returns success with path and travel time`` () =
    let destination =
        Queries.World.placesByTypeInCity Prague PlaceTypeIndex.Airport
        |> List.head

    let result = Car.planWithinCityDrive state destination

    match result with
    | Ok(path, travelTime) ->
        path |> should not' (be Empty)
        travelTime |> should be (greaterThan 0<minute>)
    | Error _ -> failwith "Expected successful planning"

[<Test>]
let ``planWithinCityDrive travel time is faster than public transport`` () =
    let destination =
        Queries.World.placesByTypeInCity Prague PlaceTypeIndex.Airport
        |> List.head

    let currentPlace = Queries.World.currentPlace state
    let currentCity = Queries.World.currentCity state

    let path =
        Navigation.Pathfinding.directionsToNode
            currentCity.Id
            currentPlace.Id
            destination.Id

    match path, Car.planWithinCityDrive state destination with
    | Some pathActions, Ok(_, carTravelTime) ->
        let publicTransportTime =
            Navigation.TravelTime.byPublicTransport pathActions

        carTravelTime |> should be (lessThan publicTransportTime)
    | _ -> failwith "Expected valid path and successful planning"

[<Test>]
let ``driveWithinCity moves character near destination place`` () =
    let destination =
        Queries.World.placesByTypeInCity Prague PlaceTypeIndex.ConcertSpace
        |> List.sample

    let effects = Car.driveWithinCity state destination carPosition testCar

    effects |> should not' (be Empty)

    effects
    |> List.exists (function
        | WorldMoveToPlace(Diff(_, (_, newPlaceId, _))) ->
            destination.Exits |> Map.head = newPlaceId
        | _ -> false)
    |> should be True

[<Test>]
let ``driveWithinCity removes car from old position and adds to new position``
    ()
    =
    let destination =
        Queries.World.placesByTypeInCity Prague PlaceTypeIndex.Airport
        |> List.head

    let effects = Car.driveWithinCity state destination carPosition testCar

    effects
    |> List.exists (function
        | ItemRemovedFromWorld(pos, car) -> pos = carPosition && car = testCar
        | _ -> false)
    |> should be True

    effects
    |> List.exists (function
        | ItemAddedToWorld(_, car) -> car = testCar
        | _ -> false)
    |> should be True

(* == Intercity drive tests == *)

[<Test>]
let ``planIntercityDrive returns AlreadyAtDestination when already in destination city``
    ()
    =
    let currentCityId, _, _ = Queries.World.currentCoordinates state

    let result = Car.planIntercityDrive state currentCityId testCar

    match result with
    | Error Car.AlreadyAtDestination -> ()
    | _ -> failwith "Expected AlreadyAtDestination error"

[<Test>]
let ``planIntercityDrive returns CannotReachDestination when no road connection exists``
    ()
    =
    // Prague to New York has no road connection (only air).
    let result = Car.planIntercityDrive state NewYork testCar

    match result with
    | Error Car.CannotReachDestination -> ()
    | _ -> failwith "Expected CannotReachDestination error"

[<Test>]
let ``planIntercityDrive returns success with distance and travel time for connected cities``
    ()
    =
    // Prague to London is connected by road (1035 km).
    let result = Car.planIntercityDrive state London testCar

    match result with
    | Ok(distance, travelTimeHours, dayMoments) ->
        distance |> should equal 1035<km>
        travelTimeHours |> should be (greaterThan 0<hour>)
        dayMoments |> should be (greaterThan 0<dayMoments>)
    | Error err -> failwith $"Expected successful planning, but got {err}"

[<Test>]
let ``planIntercityDrive calculates travel time based on car power`` () =
    let lowPowerCar, _ = Vehicles.Car.toyotaCorolla // 169 HP.
    let highPowerCar, _ = Vehicles.Car.porsche911 // 443 HP.

    let lowPowerResult = Car.planIntercityDrive state London lowPowerCar
    let highPowerResult = Car.planIntercityDrive state London highPowerCar

    match lowPowerResult, highPowerResult with
    | Ok(_, lowPowerTime, _), Ok(_, highPowerTime, _) ->
        // Higher power car should be faster.
        highPowerTime |> should be (lessThanOrEqualTo lowPowerTime)
    | _ -> failwith "Expected successful planning for both cars"

[<Test>]
let ``planIntercityDrive travel time scales with distance`` () =
    // Madrid is 1780 km from Prague.
    let longDistanceResult = Car.planIntercityDrive state Madrid testCar
    // London is 1035 km from Prague.
    let shortDistanceResult = Car.planIntercityDrive state London testCar

    match longDistanceResult, shortDistanceResult with
    | Ok(longDistance, longTime, _), Ok(shortDistance, shortTime, _) ->
        longDistance |> should be (greaterThan shortDistance)
        longTime |> should be (greaterThan shortTime)
    | _ -> failwith "Expected successful planning for both destinations"

[<Test>]
let ``driveToCity moves character to first street of destination city`` () =
    let destinationCityId = London
    let tripDuration = 10<dayMoments>

    let effects =
        Car.driveToCity state destinationCityId carPosition testCar tripDuration

    effects
    |> List.exists (function
        | WorldMoveToPlace(Diff(_, (cityId, _, _))) ->
            cityId = destinationCityId
        | _ -> false)
    |> should be True

[<Test>]
let ``driveToCity removes car from old position and adds to new city`` () =
    let destinationCityId = London
    let tripDuration = 10<dayMoments>

    let effects =
        Car.driveToCity state destinationCityId carPosition testCar tripDuration

    effects
    |> List.exists (function
        | ItemRemovedFromWorld(pos, car) -> pos = carPosition && car = testCar
        | _ -> false)
    |> should be True

    effects
    |> List.exists (function
        | ItemAddedToWorld((cityId, _, _), car) ->
            cityId = destinationCityId && car = testCar
        | _ -> false)
    |> should be True

[<Test>]
let ``driveToCity advances time by trip duration`` () =
    let destinationCityId = London
    let tripDuration = 10<dayMoments>

    let effects =
        Car.driveToCity state destinationCityId carPosition testCar tripDuration

    let advanceTimeEffects =
        effects
        |> List.filter (function
            | TimeAdvanced _ -> true
            | _ -> false)

    advanceTimeEffects |> should not' (be Empty)

[<Test>]
let ``driveToCity refreshes character attributes during trip`` () =
    let destinationCityId = Madrid
    let tripDuration = 15<dayMoments>

    let effects =
        Car.driveToCity state destinationCityId carPosition testCar tripDuration

    // Should have attribute refresh effects to prevent character depletion.
    effects
    |> List.exists (function
        | CharacterAttributeChanged(_, CharacterAttribute.Energy, _) -> true
        | _ -> false)
    |> should be True

    effects
    |> List.exists (function
        | CharacterAttributeChanged(_, CharacterAttribute.Hunger, _) -> true
        | _ -> false)
    |> should be True

    effects
    |> List.exists (function
        | CharacterAttributeChanged(_, CharacterAttribute.Mood, _) -> true
        | _ -> false)
    |> should be True

[<Test>]
let ``driveToCity generates one refresh per day moment`` () =
    let destinationCityId = London
    let tripDuration = 5<dayMoments>

    let effects =
        Car.driveToCity state destinationCityId carPosition testCar tripDuration

    let advanceTimeEffects =
        effects
        |> List.filter (function
            | TimeAdvanced _ -> true
            | _ -> false)
        |> List.length

    // Should have one AdvancedTime effect per day moment.
    advanceTimeEffects |> should equal 5

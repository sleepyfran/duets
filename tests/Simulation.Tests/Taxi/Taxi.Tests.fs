module Duets.Simulation.Tests.Taxi

open Duets.Data.World
open Duets.Simulation.Vehicles
open NUnit.Framework
open FsUnit
open Test.Common.Generators
open Aether

open Duets.Common
open Duets.Entities
open Duets.Simulation

let currentPlace =
    Queries.World.placesByTypeInCity Prague PlaceTypeIndex.Restaurant
    |> List.find (fun place -> place.Name = "Bistro Stromovka")

let state =
    State.generateOne
        { State.defaultOptions with
            CharacterFundsMin = 100m<dd> }
    |> State.World.move Prague currentPlace.Id Ids.Common.bar

[<Test>]
let ``bookRide returns AlreadyAtDestination when already at destination`` () =
    let currentPlace = Queries.World.currentPlace state

    let result = Taxi.bookRide state currentPlace

    match result with
    | Error Taxi.AlreadyAtDestination -> ()
    | _ -> failwith "Expected AlreadyAtDestination error"

[<Test>]
let ``bookRide returns CannotReachDestination when places are in different cities``
    ()
    =
    let placeInDifferentCity =
        Queries.World.placesByTypeInCity London PlaceTypeIndex.Hotel
        |> List.head

    let result = Taxi.bookRide state placeInDifferentCity

    match result with
    | Error Taxi.CannotReachDestination -> ()
    | _ -> failwith "Expected CannotReachDestination error"

[<Test>]
let ``bookRide returns NotEnoughFunds when character doesn't have enough money``
    ()
    =
    let destination =
        Queries.World.placesByTypeInCity Prague PlaceTypeIndex.Airport
        |> List.head

    let characterAccountHolder = Queries.Bank.playableCharacterAccount state

    let stateWithNoMoney =
        state
        |> Optic.map
            (Lenses.FromState.BankAccount.balanceOf_ characterAccountHolder)
            (fun _ -> 0m<dd>)

    let result = Taxi.bookRide stateWithNoMoney destination

    match result with
    | Error(Taxi.NotEnoughFunds fare) -> fare |> should be (greaterThan 0m<dd>)
    | _ -> failwith "Expected NotEnoughFunds error"

[<Test>]
let ``bookRide returns success with fare and effects when character has enough money``
    ()
    =
    let destination =
        Queries.World.placesByTypeInCity Prague PlaceTypeIndex.ConcertSpace
        |> List.sample

    let result = Taxi.bookRide state destination

    match result with
    | Ok(fare, travelTime, effects) ->
        fare |> should be (greaterThanOrEqualTo 5m<dd>)
        travelTime |> should be (greaterThan 0<minute>)
        effects |> should not' (be Empty)

        effects
        |> List.exists (function
            | WorldMoveToPlace _ -> true
            | Wait _ -> true
            | _ -> false)
        |> should be True

        effects
        |> List.exists (function
            | MoneyTransferred _ -> true
            | _ -> false)
        |> should be True
    | Error _ -> failwith "Expected successful booking"

[<Test>]
let ``bookRide calculates fare based on travel time`` () =
    let destination =
        Queries.World.placesByTypeInCity Prague PlaceTypeIndex.Airport
        |> List.head

    let result = Taxi.bookRide state destination

    match result with
    | Ok(fare, _, _) -> fare |> should equal 52.8m<dd>
    | Error err -> failwith $"Expected successful booking, but got {err}"

[<Test>]
let ``bookRide calculates fare based on city's cost of living`` () =
    let currentPlace =
        Queries.World.placesByTypeInCity LosAngeles PlaceTypeIndex.MetroStation
        |> List.find (fun place -> place.Name = "Hollywood/Vine Station")

    let state =
        State.generateOne
            { State.defaultOptions with
                CharacterFundsMin = 200m<dd> }
        |> State.World.move LosAngeles currentPlace.Id Ids.Metro.platform

    let destination =
        Queries.World.placesByTypeInCity LosAngeles PlaceTypeIndex.Airport
        |> List.head

    let result = Taxi.bookRide state destination

    match result with
    | Ok(fare, _, _) -> fare |> should equal 179.55m<dd>
    | Error err -> failwith $"Expected successful booking, but got {err}"


[<Test>]
let ``bookRide uses minimum fare for short trips`` () =
    let nearbyPlace =
        Queries.World.placesByTypeInCity Prague PlaceTypeIndex.ConcertSpace
        |> List.find (fun place -> place.Name = "Tipsport Arena")

    let result = Taxi.bookRide state nearbyPlace

    match result with
    | Ok(fare, _, _) -> fare |> should equal 5m<dd>
    | Error _ -> failwith "Expected successful booking"

[<Test>]
let ``bookRide travel time is faster than regular walking`` () =
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

    match path, Taxi.bookRide state destination with
    | Some pathActions, Ok(_, taxiTravelTime, _) ->
        let regularTravelTime =
            Navigation.TravelTime.byPublicTransport pathActions

        taxiTravelTime |> should be (lessThanOrEqualTo regularTravelTime)
    | _ -> failwith "Expected valid path and successful booking"

[<Test>]
let ``bookRide deducts fare from character's account`` () =
    let destination =
        Queries.World.placesByTypeInCity Prague PlaceTypeIndex.Airport
        |> List.head

    let result = Taxi.bookRide state destination

    match result with
    | Ok(fare, _, effects) ->
        let amountDeducted =
            effects
            |> List.choose (function
                | MoneyTransferred(_, Outgoing(amount, _)) -> Some(amount)
                | _ -> None)
            |> List.head

        amountDeducted |> should equal 52.8m<dd>
        amountDeducted |> should equal fare
    | Error err -> failwith $"Expected successful booking, but got {err}"

[<Test>]
let ``bookRide moves character near destination place`` () =
    let destination =
        Queries.World.placesByTypeInCity Prague PlaceTypeIndex.Airport
        |> List.head

    let result = Taxi.bookRide state destination

    match result with
    | Ok(_, _, effects) ->
        effects
        |> List.filter (function
            | WorldMoveToPlace(Diff(_, (_, newPlaceId, _))) ->
                destination.Exits |> Map.head = newPlaceId
            | _ -> false)
        |> should haveLength 1
    | Error _ -> failwith "Expected successful booking"

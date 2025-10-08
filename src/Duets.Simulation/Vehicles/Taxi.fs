module Duets.Simulation.Vehicles.Taxi

open Duets.Common
open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Bank.Operations
open Duets.Simulation.Navigation

type TaxiRiderError =
    | AlreadyAtDestination
    | CannotReachDestination
    | NotEnoughFunds of Amount

/// Minimum fare for any taxi ride.
let private minimumFare = 5m<dd>

/// Calculates the cost of a taxi ride based on the path to take.
let private calculateFare city travelTime =
    let calculatedFare =
        decimal travelTime
        * Config.Travel.taxiRatePerMinute
        * decimal city.CostOfLiving

    max calculatedFare minimumFare

/// Books a taxi ride from the current location to the destination place.
/// Returns the fare, path, and effects if successful, or an error if the
/// player doesn't have enough money.
let bookRide state (destination: Place) =
    let currentPlace = Queries.World.currentPlace state
    let currentCity = Queries.World.currentCity state

    if currentPlace.Id = destination.Id then
        Error(AlreadyAtDestination)
    else
        let path =
            Pathfinding.directionsToNode
                currentCity.Id
                currentPlace.Id
                destination.Id

        match path with
        | None -> Error(CannotReachDestination)
        | Some path ->
            let travelTime = TravelTime.byTaxi path
            let fare = calculateFare currentCity travelTime
            let characterAccount = Queries.Bank.playableCharacterAccount state

            expense state characterAccount fare
            |> Result.mapError (fun _ -> NotEnoughFunds fare)
            |> Result.map (fun paymentEffects ->
                let streetIdNearPlace = destination.Exits |> Map.head

                let travelEffect =
                    Navigation.moveTo streetIdNearPlace state
                    |> Result.defaultValue (Wait 0<dayMoments>)

                (fare, travelTime, travelEffect :: paymentEffects))

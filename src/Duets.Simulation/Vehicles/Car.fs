module Duets.Simulation.Vehicles.Car

open Duets.Common
open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Navigation

type CarDriveError =
    | AlreadyAtDestination
    | CannotReachDestination

/// Plans a car drive from the current location to the destination place.
/// Returns the path and travel time if successful, or an error if the
/// destination cannot be reached.
let planDrive state (destination: Place) =
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
            let travelTime = TravelTime.byCar path
            Ok(path, travelTime)

/// Executes a car drive to the destination place, moving the character to
/// the closest city of the destination and the car with them.
let drive state (destination: Place) currentCarPosition car =
    let streetIdNearPlace = destination.Exits |> Map.head

    let travelEffect =
        Navigation.moveTo streetIdNearPlace state
        |> Result.defaultValue (Wait 0<dayMoments>)

    let cityId, _, _ = Queries.World.currentCoordinates state

    let streetPath =
        Queries.World.findPlaceStreetPart
            cityId
            destination.Id
            streetIdNearPlace

    let finalCarPosition = (cityId, streetIdNearPlace, streetPath)

    [ travelEffect
      ItemRemovedFromWorld(currentCarPosition, car)
      ItemAddedToWorld(finalCarPosition, car) ]

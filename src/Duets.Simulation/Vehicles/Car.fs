module Duets.Simulation.Vehicles.Car

open Duets.Common
open Duets.Data.World
open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Character
open Duets.Simulation.Navigation

type CarDriveError =
    | AlreadyAtDestination
    | CannotReachDestination

/// Plans a car drive from the current location to the destination place within
/// the same city. Returns the path and travel time if successful, or an error
/// if the destination cannot be reached.
let planWithinCityDrive state (destination: Place) =
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

/// Executes a car drive to the destination place within the same city, moving
/// the character to the closest street of the destination and the car with them.
let driveWithinCity state (destination: Place) currentCarPosition car =
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

/// Plans a car drive from the current city to a destination city connected by road.
/// Returns the distance and travel time if successful, or an error if the destination
/// cannot be reached by road.
let planIntercityDrive state destinationCityId (car: Item) =
    let currentCityId, _, _ = Queries.World.currentCoordinates state

    if currentCityId = destinationCityId then
        Error(AlreadyAtDestination)
    else
        let destinationConnection =
            World.citiesReachableByRoadFrom currentCityId
            |> List.tryFind (fun (cityId, _) -> cityId = destinationCityId)

        match destinationConnection with
        | None -> Error(CannotReachDestination)
        | Some(_, distance) ->
            let carProperty = car |> Item.Property.tryMain

            let averageSpeed =
                match carProperty with
                | Some(ItemProperty.Rideable(RideableItem.Car(car))) ->
                    // Calculate average speed based on horsepower.
                    let baseSpeed = 100.0
                    let horsepowerValue = float (car.Power / 1<horsepower>)
                    let horsepowerBonus = horsepowerValue * 0.15
                    min 220.0 (baseSpeed + horsepowerBonus)
                | _ ->
                    // For some reason we're in a non-car vehicle, assume 120.
                    120.0

            let travelTimeHours =
                (float distance) / averageSpeed
                |> Math.roundToNearest
                |> (*) 1<hour>

            let dayMomentsToAdvance =
                max 1<dayMoments> ((travelTimeHours / 1<hour>) * 1<dayMoments>)

            Ok(distance, travelTimeHours, dayMomentsToAdvance)

/// Executes a car drive to a destination city, moving the character to the first
/// street in that city and the car with them. Time is advanced with periodic
/// attribute refreshes to prevent the character from depleting during long trips.
let driveToCity state destinationCityId currentCarPosition car tripDuration =
    let currentCoords = Queries.World.currentCoordinates state

    let destinationCity = Queries.World.cityById destinationCityId

    let firstStreetId =
        destinationCity.StreetIndex |> Map.toList |> List.head |> fst

    let destinationCoords = (destinationCityId, firstStreetId, "0")

    let currentDate = Queries.Calendar.today state

    (*
    Refill energy, mood and hunger on each day moment advance to keep the
    character from being sent to the hospital upon arrival.
    TODO: Come up with a better way of doing this? Maybe "simulate" an overnight stay at hotels and stops at restaurants?
    *)
    let advanceTimeEffects =
        Time.AdvanceTime.advanceDayMoment currentDate tripDuration
        |> List.collect (fun timeEffect ->
            [ timeEffect
              yield! Attribute.setToPlayable CharacterAttribute.Energy 60 state
              yield! Attribute.setToPlayable CharacterAttribute.Hunger 60 state
              yield! Attribute.setToPlayable CharacterAttribute.Mood 60 state ])

    // Movement and car repositioning effects
    let moveEffect = WorldMoveToPlace(Diff(currentCoords, destinationCoords))

    let carRemoveEffect = ItemRemovedFromWorld(currentCarPosition, car)
    let carPlaceEffect = ItemAddedToWorld(destinationCoords, car)

    [ moveEffect; carRemoveEffect; carPlaceEffect ] @ advanceTimeEffects

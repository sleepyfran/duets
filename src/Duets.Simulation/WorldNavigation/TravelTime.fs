[<RequireQualifiedAccess>]
module Duets.Simulation.Navigation.TravelTime

open Duets.Common
open Duets.Entities
open Duets.Simulation

/// Returns the approximate travel time it takes to perform the given path by
/// walk/public transport.
let byPublicTransport path =
    (0<minute>, path)
    ||> List.fold (fun acc action ->
        let hintTime =
            match action with
            | Pathfinding.Enter _
            | Pathfinding.GoOut _ -> 3<minute>
            | Pathfinding.TakeMetro _ ->
                Config.Time.travelTimeBetweenDifferentZones
            | Pathfinding.Walk _ -> Config.Time.travelTimeBetweenSameZone

        acc + hintTime)

/// Returns the approximate travel time it takes to perform the given path by
/// taxi.
let byTaxi path =
    let regularTravelTime = byPublicTransport path
    float regularTravelTime / 2.0 |> Math.roundToNearest |> (*) 1<minute>

/// Returns the approximate travel time it takes to perform the given path by
/// car (similar to taxi but player is driving).
let byCar path =
    let regularTravelTime = byPublicTransport path
    float regularTravelTime / 2.0 |> Math.roundToNearest |> (*) 1<minute>

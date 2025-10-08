module Duets.Simulation.Navigation.TravelTime

open Duets.Entities
open Duets.Simulation

/// Returns the approximate travel time it takes to perform the given path.
let travelTime path =
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

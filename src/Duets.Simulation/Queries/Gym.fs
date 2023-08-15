namespace Duets.Simulation.Queries

open Duets.Entities

module Gym =
    /// Calculates the cost of entering a gym based on the quality of the place
    /// and the cost of living in the city.
    let calculateEntranceCost cityId (place: Place) =
        let city = World.cityById cityId

        let baseCost =
            match place.Quality with
            | q when q < 25<quality> -> 0.1m<dd>
            | q when q < 50<quality> -> 0.3m<dd>
            | q when q < 75<quality> -> 0.5m<dd>
            | _ -> 0.8m<dd>

        baseCost * (decimal city.CostOfLiving)

namespace Duets.Simulation.Navigation

open Duets.Common
open Duets.Entities
open Duets.Simulation

module Navigation =
    /// Moves the player to the specific place ID.
    let moveTo placeId state =
        let currentCity = Queries.World.currentCity state

        Navigation.Policies.OpeningHours.canEnter state currentCity.Id placeId
        |> Result.transform (WorldMoveTo(currentCity.Id, placeId))

    /// Moves the player to the specific place ID inside the given city ID.
    let travelTo cityId placeId = WorldMoveTo(cityId, placeId)

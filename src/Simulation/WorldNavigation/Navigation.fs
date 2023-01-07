namespace Simulation.Navigation

open Common
open Entities
open Simulation

module Navigation =
    /// Moves the player to the specific place ID.
    let moveTo placeId state =
        let currentCity = Queries.World.currentCity state

        Navigation.Policies.OpeningHours.canEnter state currentCity.Id placeId
        |> Result.transform (WorldMoveTo(currentCity.Id, placeId))

    /// Moves the player to the specific place ID inside the given city ID.
    let travelTo cityId placeId = WorldMoveTo(cityId, placeId)

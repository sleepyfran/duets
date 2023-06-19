namespace Duets.Simulation.Navigation

open Duets.Common
open Duets.Data
open Duets.Entities
open Duets.Simulation

module Navigation =
    /// Moves the player to the specific place ID.
    let moveTo placeId roomId state =
        let currentCity = Queries.World.currentCity state

        Navigation.Policies.OpeningHours.canEnter state currentCity.Id placeId
        |> Result.andThen (
            Navigation.Policies.Rental.canEnter state currentCity.Id placeId
        )
        |> Result.transform (WorldMoveTo(currentCity.Id, placeId, roomId))

    /// Moves the player to the specific place ID inside the given city ID.
    let travelTo cityId placeId =
        WorldMoveTo(cityId, placeId, World.Ids.Airport.lobby)

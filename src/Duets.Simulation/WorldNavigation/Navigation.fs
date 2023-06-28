namespace Duets.Simulation.Navigation

open Duets.Common
open Duets.Data
open Duets.Entities
open Duets.Simulation

module Navigation =
    let private applyPolicies state cityId placeId =
        Navigation.Policies.OpeningHours.canEnter state cityId placeId
        |> Result.andThen (
            Navigation.Policies.Rental.canEnter state cityId placeId
        )

    /// Moves the player to the specific place ID.
    let moveTo placeId state =
        let cityId, _, _ = Queries.World.currentCoordinates state

        applyPolicies state cityId placeId
        |> Result.transform (WorldMoveTo(cityId, placeId, 0))

    /// Moves the player to the specified room inside of the current place.
    let enter roomId state =
        let cityId, placeId, _ = Queries.World.currentCoordinates state

        Navigation.Policies.Concert.canEnter state cityId placeId roomId
        |> Result.transform (WorldEnter(cityId, placeId, roomId))

    /// Moves the player to the specific place ID inside the given city ID.
    let travelTo cityId placeId =
        WorldMoveTo(cityId, placeId, World.Ids.Airport.lobby)

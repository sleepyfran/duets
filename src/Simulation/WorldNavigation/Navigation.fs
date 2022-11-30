namespace Simulation.Navigation


open Entities
open Simulation

module Navigation =
    /// Moves the player to the specific place ID.
    let moveTo placeId state =
        let currentCity =
            Queries.World.currentCity state

        WorldMoveTo(currentCity.Id, placeId)

    /// Moves the player to the specific place ID inside the given city ID.
    let travelTo cityId placeId = WorldMoveTo(cityId, placeId)

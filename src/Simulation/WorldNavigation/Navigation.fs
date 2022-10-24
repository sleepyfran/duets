namespace Simulation.Navigation


open Entities
open Simulation

module Navigation =
    /// Moves the player to the specific node ID.
    let moveTo placeId state =
        let currentCity =
            Queries.World.currentCity state

        WorldMoveTo(currentCity.Id, placeId)

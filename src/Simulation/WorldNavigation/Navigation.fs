namespace Simulation.Navigation


open Common
open Entities
open Simulation

module Navigation =
    /// Moves the player to the specific node ID.
    let moveTo coordinates state =
        let currentPosition =
            Queries.World.Common.currentPosition state

        let destinationCoords =
            Queries.World.Common.coordinates state coordinates

        match destinationCoords.Content with
        | ResolvedPlaceCoordinates coords ->
            Navigation.Policies.Concert.canEnter state coords
        | ResolvedOutsideCoordinates _ -> Ok()
        |> Result.transform (WorldMoveTo(currentPosition.City.Id, coordinates))

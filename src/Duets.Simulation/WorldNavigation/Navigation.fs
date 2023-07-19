namespace Duets.Simulation.Navigation

open Duets.Common
open Duets.Data
open Duets.Entities
open Duets.Simulation

module Navigation =
    let private applyPlacePolicies state cityId placeId =
        [ Navigation.Policies.OpeningHours.canMove
          Navigation.Policies.Rental.canMove ]
        |> List.fold
            (fun result policy ->
                result |> Result.bind (fun _ -> policy state cityId placeId))
            (Ok())

    let private applyRoomPolicies state cityId placeId roomId =
        [ Navigation.Policies.Concert.canEnter
          Navigation.Policies.Rental.canEnter ]
        |> List.fold
            (fun result policy ->
                result
                |> Result.bind (fun _ -> policy state cityId placeId roomId))
            (Ok())

    /// Moves the player to the specific place ID.
    let moveTo placeId state =
        let cityId, _, _ = Queries.World.currentCoordinates state
        let place = Queries.World.placeInCityById cityId placeId
        let startingRoom = place.Rooms.StartingNode

        applyPlacePolicies state cityId placeId
        |> Result.transform (WorldMoveTo(cityId, placeId, startingRoom))

    /// Moves the player to the specified room inside of the current place.
    let enter roomId state =
        let cityId, placeId, _ = Queries.World.currentCoordinates state

        applyRoomPolicies state cityId placeId roomId
        |> Result.transform (WorldEnter(cityId, placeId, roomId))

    /// Moves the player to the specific place ID inside the given city ID.
    let travelTo cityId placeId =
        WorldMoveTo(cityId, placeId, World.Ids.Airport.lobby)

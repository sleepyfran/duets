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

    let private applyRoomPolicies
        state
        cityId
        placeId
        currentRoomId
        nextRoomId
        =
        [ (Navigation.Policies.Room.canEnter currentRoomId)
          Navigation.Policies.Concert.canEnter
          Navigation.Policies.Rental.canEnter ]
        |> List.fold
            (fun result policy ->
                result
                |> Result.bind (fun _ ->
                    policy state cityId placeId nextRoomId))
            (Ok())

    /// Moves the player to the specific place ID.
    let moveTo placeId state =
        let currentCoords = Queries.World.currentCoordinates state
        let cityId, _, _ = currentCoords
        let place = Queries.World.placeInCityById cityId placeId
        let startingRoom = place.Rooms.StartingNode

        applyPlacePolicies state cityId placeId
        |> Result.transform (
            WorldMoveToPlace(
                Diff(currentCoords, (cityId, placeId, startingRoom))
            )
        )

    /// Moves the player to the specified room inside of the current place.
    let enter roomId state =
        let currentCoords = Queries.World.currentCoordinates state
        let cityId, placeId, currentRoomId = currentCoords

        applyRoomPolicies state cityId placeId currentRoomId roomId
        |> Result.transform (
            WorldEnterRoom(Diff(currentCoords, (cityId, placeId, roomId)))
        )

    /// Moves the player to the specific place ID inside the given city ID.
    let travelTo cityId placeId state =
        let currentCoords = Queries.World.currentCoordinates state

        WorldMoveToPlace(
            Diff(currentCoords, (cityId, placeId, World.Ids.Common.lobby))
        )

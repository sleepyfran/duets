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

    /// Moves the player to the specified street inside of the current city.
    let exitTo streetId state =
        let currentCoords = Queries.World.currentCoordinates state
        let currentPlace = Queries.World.currentPlace state

        let cityId, _, _ = currentCoords
        let street = Queries.World.streetInCurrentCity streetId state

        // Streets can be partitioned into multiple parts (to have "Street
        // continues to..."), so when exiting from a building attempt to leave
        // the player in the part of the street that connects to the building,
        // otherwise it's a bit confusing.
        let streetPart =
            match street.Type with
            | StreetType.OneWay -> "0"
            | StreetType.Split(_, splits) ->
                // TODO: Please write tests for all street related stuff, it's really fragile!
                let currentPlaceIndex =
                    street.Places
                    |> List.findIndex (fun place -> place.Id = currentPlace.Id)

                let itemsPerGroup = street.Places.Length / splits
                let idx = float currentPlaceIndex / float itemsPerGroup
                idx - 1.0 |> Math.ceilToNearest |> string

        // Streets are not "real" places, but we index them like them via
        // their street ID.
        WorldMoveToPlace(Diff(currentCoords, (cityId, streetId, streetPart)))

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

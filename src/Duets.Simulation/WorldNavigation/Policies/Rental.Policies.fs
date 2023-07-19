module Duets.Simulation.Navigation.Policies.Rental

open Aether
open Duets.Common
open Duets.Entities
open Duets.Simulation

/// Returns whether the character can enter in a place by checking if they
/// are actually renting it currently or not.
let canMove state cityId placeId =
    let place = (cityId, placeId) ||> Queries.World.placeInCityById
    let placeRental = (cityId, placeId) |> Queries.Rentals.getForCoords state

    match place.Type, placeRental with
    | PlaceType.Home, None -> false
    | _ -> true
    |> Result.ofBool PlaceEntranceError.CannotEnterWithoutRental

/// Returns whether the character can enter in a room by checking if they
/// are actually renting it currently or not.
let canEnter state cityId placeId roomId =
    let place = Queries.World.placeInCityById cityId placeId
    let room = Queries.World.roomById cityId placeId roomId
    let placeRental = (cityId, placeId) |> Queries.Rentals.getForCoords state

    match place.Type, placeRental with
    | PlaceType.Hotel _, None when room = RoomType.Bedroom -> false
    | _ -> true
    |> Result.ofBool RoomEntranceError.CannotEnterHotelRoomWithoutBooking

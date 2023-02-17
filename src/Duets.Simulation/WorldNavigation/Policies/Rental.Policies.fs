module Duets.Simulation.Navigation.Policies.Rental

open Aether
open Duets.Common
open Duets.Entities
open Duets.Simulation

/// Returns whether the character can enter in a place by checking if they
/// are actually renting it currently or not.
let canEnter state cityId placeId =
    let place = (cityId, placeId) ||> Queries.World.placeInCityById
    let placeRental = (cityId, placeId) |> Queries.Rentals.getForCoords state

    match place.Type, placeRental with
    | PlaceType.Home, None -> false
    | _ -> true
    |> Result.ofBool PlaceEntranceError.CannotEnterWithoutRental

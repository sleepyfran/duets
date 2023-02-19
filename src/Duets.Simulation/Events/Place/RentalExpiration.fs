module Duets.Simulation.Events.Place.RentalExpiration

open Duets.Entities
open Duets.Simulation

let private checkPlaceRental state cityId (place: Place) =
    let placeRental = Queries.Rentals.getForCoords state (cityId, place.Id)

    match placeRental with
    | Some _ -> []
    | None -> [ RentalExpired place ]

/// Checks if the current place requires any sort of rental and if so, checks
/// that the character still holds the rental required to be here.
let checkCurrentPlace state =
    let currentCity = Queries.World.currentCity state
    let currentPlace = Queries.World.currentPlace state

    let requiresRental =
        match currentPlace.Type with
        | PlaceType.Home -> true
        | _ -> false

    if requiresRental then
        checkPlaceRental state currentCity.Id currentPlace
    else
        []

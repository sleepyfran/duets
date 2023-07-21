module Duets.Simulation.Events.Place.RentalExpiration

open Duets.Entities
open Duets.Simulation

let rec private expireRentalIfNeeded state rental currentDate untilDate =
    let currentPlace = Queries.World.currentPlace state
    let expiredPlace = rental.Coords ||> Queries.World.placeInCityById

    if currentDate > untilDate then
        [ RentalExpired rental

          (* If the player is currently here, kick them out! *)
          if currentPlace = expiredPlace then
              RentalKickedOut rental ]
    else
        []

let private checkPlaceRental state currentDate rental =
    match rental.RentalType with
    | Monthly untilDate
    | OneTime(_, untilDate) ->
        expireRentalIfNeeded state rental currentDate untilDate

/// Checks if any of the rentals that the player has have expired and, if so,
/// returns a RentalExpired effect.
let expireRentals state currentDate =
    Queries.Rentals.all state
    |> List.fold
        (fun acc rental -> acc @ checkPlaceRental state currentDate rental)
        []

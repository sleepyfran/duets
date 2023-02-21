module Duets.Simulation.Events.Place.RentalExpiration

open Duets.Entities
open Duets.Simulation

let rec private expireRentalIfNeeded rental currentDate untilDate =
    if currentDate > untilDate then
        [ RentalExpired rental ]
    else
        []

let private checkPlaceRental currentDate rental =
    match rental.RentalType with
    | Monthly untilDate
    | OneTime untilDate -> expireRentalIfNeeded rental currentDate untilDate

/// Checks if the current place requires any sort of rental and if so, checks
/// that the character still holds the rental required to be here.
let checkCurrentPlace state =
    let currentDate =
        Queries.Calendar.today state |> Calendar.Transform.resetDayMoment

    Queries.Rentals.all state
    |> List.fold (fun _ -> checkPlaceRental currentDate) []

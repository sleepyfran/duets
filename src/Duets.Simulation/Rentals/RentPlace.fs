module Duets.Simulation.Rentals.RentPlace

open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Bank.Operations
open FsToolkit.ErrorHandling

type RentError =
    | PlaceNotAvailable
    | TransactionError of TransactionError

/// Attempts to rent the given place for the character. If the place is not available
/// for rent or if the character does not have enough money, returns an error.
let rentPlace state cityId (place: Place) =
    match place.Type with
    | Home ->
        let characterAccount = Queries.Bank.playableCharacterAccount state
        let rentalPrice = Queries.Rentals.calculateRentalPrice cityId place
        let today = Queries.Calendar.today state

        result {
            let! expenseEffects =
                rentalPrice
                |> expense state characterAccount
                |> Result.mapError TransactionError

            let rental =
                { Amount = rentalPrice
                  Coords = cityId, place.Id
                  RentalType = Monthly today }

            return rental, expenseEffects @ [ RentalAdded rental ]
        }
    | _ -> Error PlaceNotAvailable

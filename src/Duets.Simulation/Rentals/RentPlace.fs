module Duets.Simulation.Rentals.RentPlace

open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Bank.Operations
open FsToolkit.ErrorHandling

type RentError =
    | PlaceNotAvailable
    | TransactionError of TransactionError

/// Creates a rental for the given city and place, with the payment date set to
/// the current date + 1 month.
let createRental currentDate cityId place =
    let rentalPrice = Queries.Rentals.calculateRentalPrice cityId place

    let nextPaymentDate = currentDate |> Calendar.Ops.addMonths 1

    { Amount = rentalPrice
      Coords = cityId, place.Id
      RentalType = Monthly nextPaymentDate }

/// Attempts to rent the given place for the character. If the place is not available
/// for rent or if the character does not have enough money, returns an error.
let rentPlace state cityId (place: Place) =
    match place.Type with
    | Home ->
        let characterAccount = Queries.Bank.playableCharacterAccount state
        let currentDate = Queries.Calendar.today state
        let rental = createRental currentDate cityId place

        result {
            let! expenseEffects =
                rental.Amount
                |> expense state characterAccount
                |> Result.mapError TransactionError

            return rental, expenseEffects @ [ RentalAdded rental ]
        }
    | _ -> Error PlaceNotAvailable

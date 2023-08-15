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
let createMonthlyRental currentDate cityId place =
    let rentalPrice = Queries.Rentals.calculateMonthlyRentalPrice cityId place

    let nextPaymentDate = currentDate |> Calendar.Ops.addMonths 1

    { Amount = rentalPrice
      Coords = cityId, place.Id
      RentalType = Monthly nextPaymentDate }

/// Creates a rental for the given city and place, with the payment date set to
/// the given date + the given number of days.
let createOneTimeRental fromDate numberOfDays cityId place =
    let rentalPrice =
        Queries.Rentals.calculateOneTimeRentalPrice place numberOfDays

    let untilDate = fromDate |> Calendar.Ops.addDays numberOfDays

    { Amount = rentalPrice
      Coords = cityId, place.Id
      RentalType = OneTime(fromDate, untilDate) }

/// Attempts to rent the given place for the character. If the place is not available
/// for rent or if the character does not have enough money, returns an error.
let rentMonthlyPlace state cityId (place: Place) =
    match place.PlaceType with
    | Home ->
        let characterAccount = Queries.Bank.playableCharacterAccount state
        let currentDate = Queries.Calendar.today state
        let rental = createMonthlyRental currentDate cityId place

        result {
            let! expenseEffects =
                rental.Amount
                |> expense state characterAccount
                |> Result.mapError TransactionError

            return rental, expenseEffects @ [ RentalAdded rental ]
        }
    | _ -> Error PlaceNotAvailable

/// Attempts to rent the given place for the character. If the place is not available
/// for rent or if the character does not have enough money, returns an error.
let rentOneTimePlace
    state
    cityId
    (place: Place)
    (fromDate: Date)
    (numberOfDays: int<days>)
    =
    match place.PlaceType with
    | Hotel _ ->
        let characterAccount = Queries.Bank.playableCharacterAccount state
        let rental = createOneTimeRental fromDate numberOfDays cityId place

        result {
            let! expenseEffects =
                rental.Amount
                |> expense state characterAccount
                |> Result.mapError TransactionError

            return rental, expenseEffects @ [ RentalAdded rental ]
        }
    | _ -> Error PlaceNotAvailable

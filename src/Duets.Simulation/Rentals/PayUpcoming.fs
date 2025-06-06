module Duets.Simulation.Rentals.PayUpcoming

open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Bank.Operations
open FsToolkit.ErrorHandling

/// Attempts to pay for an upcoming seasonal rental, expensing the money from the
/// character's account and then updating the rental's next payment date to
/// next season.
let payRental state (rental: Rental) =
    let characterAccount = Queries.Bank.playableCharacterAccount state

    result {
        let! expenseEffects = rental.Amount |> expense state characterAccount

        let nextPaymentDate = Rental.dueDate rental |> Calendar.Ops.addSeasons 1

        let updatedRental =
            { rental with
                RentalType = Seasonal nextPaymentDate }

        return updatedRental, expenseEffects @ [ RentalUpdated updatedRental ]
    }

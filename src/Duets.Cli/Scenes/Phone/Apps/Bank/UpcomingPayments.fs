module rec Duets.Cli.Scenes.Phone.Apps.Bank.UpcomingPayments

open Duets.Agents
open Duets.Cli
open Duets.Cli.Components
open Duets.Cli.Text
open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Bank.Operations
open Duets.Simulation.Rentals.PayUpcoming

/// Displays a list of all the upcoming payments in the next two weeks and allows
/// the player to pay them.
let upcomingPayments bankApp (upcoming: Rental list) =
    showOptionalChoicePrompt
        "Which rental do you want to pay?"
        Generic.nothing
        (fun rental ->
            let cityId, _ = rental.Coords
            let place = rental.Coords ||> Queries.World.placeInCityById
            let dueDate = rental |> Rental.dueDate

            $"{place.Name} in {place.Zone.Name}, {Generic.cityName cityId}. Amount: {Styles.money rental.Amount}, due on {Generic.dateWithDay dueDate}")
        upcoming
    |> Option.iter confirmPayment

    bankApp ()

let private confirmPayment selectedRental =
    let confirmed =
        showConfirmationPrompt
            $"Do you really want to pay {Styles.money selectedRental.Amount}?"

    if confirmed then
        let result = payRental (State.get ()) selectedRental

        match result with
        | Ok(rental, effects) ->
            let nextPaymentDate = Rental.dueDate rental

            Styles.success
                $"You paid for another month of your rental. Your next payment date is {Generic.dateWithDay nextPaymentDate}"
            |> showMessage

            effects |> Effect.applyMultiple
        | Error(NotEnoughFunds _) ->
            Styles.error
                "You don't have enough money in your account to pay for this"
            |> showMessage

module Duets.Cli.Scenes.Phone.Apps.Bank.LoanRequest

open Duets.Agents
open Duets.Cli
open Duets.Cli.Components
open Duets.Cli.Text
open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Bank.LoanOperations

/// Displays the loan request flow, allowing the player to borrow a fixed
/// tier amount at the rate determined by their bank reputation.
let requestLoan bankApp =
    let state = State.get ()
    let loanState = Queries.Bank.loanStateOf state

    let rate = Queries.Bank.interestRateForReputation loanState.Reputation

    let selection =
        showOptionalChoicePrompt
            Phone.bankAppLoanSelectAmount
            Generic.back
            (fun amount ->
                let payment =
                    Queries.Bank.seasonalPayment
                        { Principal = amount
                          InterestRate = rate
                          LastPaymentDate = state.Today
                          MissedPayments = 0 }

                let ratePercent = int (rate * 100.0)
                $"{Styles.money amount} — {ratePercent}%% PA, seasonal payment: {Styles.money payment}")
            Config.Loan.availableAmounts

    match selection with
    | None -> bankApp ()
    | Some amount ->
        let payment =
            Queries.Bank.seasonalPayment
                { Principal = amount
                  InterestRate = rate
                  LastPaymentDate = state.Today
                  MissedPayments = 0 }

        let confirmed =
            showConfirmationPrompt
                (Phone.bankAppLoanConfirmation amount rate payment)

        if confirmed then
            match takeLoan state amount with
            | Ok effects ->
                Phone.bankAppLoanSuccess amount |> showMessage
                effects |> Effect.applyMultiple
            | Error AlreadyHasLoan -> ()
            | Error IsBlacklisted -> ()
            | Error(NotEnoughFundsForPayoff _) -> ()

        bankApp ()

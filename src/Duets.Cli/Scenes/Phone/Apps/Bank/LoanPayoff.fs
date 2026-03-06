module Duets.Cli.Scenes.Phone.Apps.Bank.LoanPayoff

open Duets.Agents
open Duets.Cli
open Duets.Cli.Components
open Duets.Cli.Text
open Duets.Simulation
open Duets.Simulation.Bank.LoanOperations

/// Displays the loan payoff confirmation and processes full repayment.
let payOffLoan bankApp =
    let state = State.get ()

    match Queries.Bank.activeLoan state with
    | None -> bankApp ()
    | Some loan ->
        let amount = Queries.Bank.payoffAmount loan

        let confirmed =
            showConfirmationPrompt (Phone.bankAppLoanPayoffConfirmation amount)

        if confirmed then
            match payOffLoan state with
            | Ok effects ->
                Phone.bankAppLoanPayoffSuccess |> showMessage
                effects |> Effect.applyMultiple
            | Error(NotEnoughFundsForPayoff needed) ->
                Phone.bankAppLoanPayoffNotEnoughFunds needed |> showMessage
            | Error AlreadyHasLoan -> ()
            | Error IsBlacklisted -> ()

        bankApp ()

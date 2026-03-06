module Duets.Simulation.Bank.LoanOperations

open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Bank.Operations
open Duets.Simulation.Queries

type LoanError =
    | AlreadyHasLoan
    | IsBlacklisted
    | NotEnoughFundsForPayoff of Amount

/// Requests a new loan for the given amount. Returns the effects to apply or
/// an error if the character already has a loan or is blacklisted.
let takeLoan state (amount: Amount) =
    let loanState = Bank.loanStateOf state

    if loanState.ActiveLoan.IsSome then
        Error AlreadyHasLoan
    elif loanState.Reputation = Blacklisted then
        Error IsBlacklisted
    else
        let rate = Bank.interestRateForReputation loanState.Reputation
        let today = Calendar.today state
        let characterAccount = Bank.playableCharacterAccount state

        let loan =
            { Principal = amount
              InterestRate = rate
              LastPaymentDate = today
              MissedPayments = 0 }

        Ok [ income state characterAccount amount; LoanTaken loan ]

/// Pays off the entire remaining principal of the active loan. Returns the
/// effects to apply or an error if there is no loan or insufficient funds.
let payOffLoan state =
    match Bank.activeLoan state with
    | None -> Error AlreadyHasLoan
    | Some loan ->
        let characterAccount = Bank.playableCharacterAccount state
        let amount = Bank.payoffAmount loan

        match expense state characterAccount amount with
        | Ok effects -> Ok(effects @ [ LoanPaidOff loan ])
        | Error(NotEnoughFunds a) -> Error(NotEnoughFundsForPayoff a)

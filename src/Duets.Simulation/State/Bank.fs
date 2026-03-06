module Duets.Simulation.State.Bank

open Aether
open Duets.Entities

let setBalance account transaction =
    let balanceLens = Lenses.FromState.BankAccount.balanceOf_ account

    let updatedBalance =
        match transaction with
        | Incoming(_, balance) -> balance
        | Outgoing(_, balance) -> balance

    Optic.set balanceLens updatedBalance

let setLoan loan state =
    Optic.set Lenses.FromState.Bank.activeLoan_ (Some loan) state

let removeLoan state =
    Optic.set Lenses.FromState.Bank.activeLoan_ None state

let setReputation reputation state =
    Optic.set Lenses.FromState.Bank.reputation_ reputation state

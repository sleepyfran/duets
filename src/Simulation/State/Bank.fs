module Simulation.State.Bank

open Aether
open Entities

let setBalance account transaction =
    let balanceLens = Lenses.FromState.BankAccount.balanceOf_ account

    let updatedBalance =
        match transaction with
        | Incoming (_, balance) -> balance
        | Outgoing (_, balance) -> balance

    Optic.set balanceLens updatedBalance

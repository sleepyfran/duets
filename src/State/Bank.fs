namespace State

module Bank =
    open Aether
    open Entities

    let setBalance map account transaction =
        let balanceLens =
            Lenses.FromState.BankAccount.balanceOf_ account

        let updatedBalance =
            match transaction with
            | Incoming (_, balance) -> balance
            | Outgoing (_, balance) -> balance

        map (Optic.set balanceLens updatedBalance)

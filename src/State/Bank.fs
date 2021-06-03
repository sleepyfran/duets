namespace State

module Bank =
    open Aether
    open Entities

    let transfer map account transaction =
        let transactionsLens =
            Lenses.FromState.BankAccount.transactionsOf_ account

        let addTransaction = List.append [ transaction ]

        map (Optic.map transactionsLens addTransaction)

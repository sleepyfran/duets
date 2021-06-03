namespace Simulation.Queries

module Bank =
    open Aether
    open Entities

    /// Returns the account balance of the given holder.
    let balanceOf state holder =
        state
        |> Optic.get (Lenses.FromState.BankAccount.transactionsOf_ holder)
        |> Option.defaultValue []
        |> List.fold
            (fun balance transaction ->
                match transaction with
                | Incoming (_, amount) -> balance + amount
                | Outgoing (_, amount) -> balance - amount)
            0<dd>

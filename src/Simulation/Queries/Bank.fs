namespace Simulation.Queries

module Bank =
    open Aether
    open Entities

    /// Returns the account balance of the given holder.
    let balanceOf state holder =
        state
        |> Optic.get (Lenses.FromState.BankAccount.balanceOf_ holder)
        |> Option.defaultValue 0<dd>

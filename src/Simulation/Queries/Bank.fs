namespace Simulation.Queries

module Bank =
    open Aether
    open Entities

    /// Returns the playable character's bank account.
    let playableCharacterAccount state =
        let character = Characters.playableCharacter state

        Character character.Id

    /// Returns the account balance of the given holder.
    let balanceOf state holder =
        state
        |> Optic.get (Lenses.FromState.BankAccount.balanceOf_ holder)
        |> Option.defaultValue 0<dd>

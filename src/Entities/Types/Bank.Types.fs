namespace Entities

[<AutoOpen>]
module BankTypes =
    /// Holder of an account in the in-game bank.
    type BankAccountHolder =
        | Character of CharacterId
        | Band of BandId

    /// Represents a transaction between two accounts in the game.
    type BankTransaction =
        | Incoming of amount: Amount * updatedBalance: Amount
        | Outgoing of amount: Amount * updatedBalance: Amount

    /// Represents a bank account in the game. We only keep track of accounts
    /// from the main character and its bands.
    type BankAccount =
        { Holder: BankAccountHolder
          Balance: Amount }

namespace Duets.Entities

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

    /// Represents the bank's assessment of the character's reliability
    /// based on their loan repayment history.
    type BankReputation =
        | GoodStanding
        | Flagged
        | Blacklisted

    /// Represents an active loan from the bank.
    type Loan =
        { Principal: Amount
          InterestRate: float
          LastPaymentDate: Date
          MissedPayments: int }

    /// Represents the loan side of the character's bank state, tracking the
    /// optional active loan and the bank reputation.
    type LoanState =
        { ActiveLoan: Loan option
          Reputation: BankReputation }

    /// Represents the full bank state: accounts for all holders and the
    /// character's loan state.
    type BankState =
        { Accounts: Map<BankAccountHolder, BankAccount>
          LoanState: LoanState }

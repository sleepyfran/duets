namespace Duets.Entities

[<AutoOpen>]
module LoanTypes =
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

    /// Represents the character's loan state, tracking the optional active
    /// loan and their bank reputation.
    type LoanState =
        { ActiveLoan: Loan option
          Reputation: BankReputation }

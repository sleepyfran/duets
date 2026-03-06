namespace Duets.Simulation.Queries

open Duets.Simulation

module Loan =
    open Aether
    open Duets.Entities

    /// Returns the character's current loan state.
    let loanState state = Optic.get Lenses.State.loan_ state

    /// Returns the active loan if any.
    let activeLoan state = (loanState state).ActiveLoan

    /// Returns the current bank reputation.
    let reputation state = (loanState state).Reputation

    /// Calculates the annual interest rate based on the given reputation.
    let interestRateForReputation reputation =
        match reputation with
        | GoodStanding -> Config.Loan.baseAnnualRate
        | Flagged -> Config.Loan.baseAnnualRate + Config.Loan.flaggedAdditionalRate
        | Blacklisted -> Config.Loan.baseAnnualRate

    /// Returns whether the character can take out a new loan.
    let canTakeLoan state =
        let ls = loanState state
        ls.ActiveLoan.IsNone && ls.Reputation <> Blacklisted

    /// Calculates the seasonal payment for a loan (principal * annualRate / 4).
    let seasonalPayment (loan: Loan) =
        loan.Principal * decimal (loan.InterestRate / 4.0)

    /// Returns the amount required to pay off the loan in full.
    let payoffAmount (loan: Loan) = loan.Principal

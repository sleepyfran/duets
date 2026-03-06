module Duets.Simulation.Events.Bank.LoanPayment

open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Bank.Operations
open Duets.Simulation.Queries

/// Called from runDailyEffects at Morning. Checks if it's the first day of a
/// season and processes the loan auto-payment if there is an active loan.
let processSeasonalPayment state (time: Date) =
    if time.Day <> 1<days> then
        []
    else
        match Bank.activeLoan state with
        | None -> []
        | Some loan ->
            let characterAccount = Bank.playableCharacterAccount state
            let payment = Bank.seasonalPayment loan
            let balance = Bank.balanceOf state characterAccount

            if balance >= payment then
                let newPrincipal = loan.Principal - payment

                if newPrincipal <= 0m<dd> then
                    [ MoneyTransferred(
                          characterAccount,
                          Outgoing(payment, balance - payment)
                      )
                      LoanPaidOff loan
                      NotificationShown(
                          Notification.LoanNotification(
                              LoanNotificationType.LoanSeasonalPaymentMade
                                  payment
                          )
                      ) ]
                else
                    let updatedLoan =
                        { loan with
                            Principal = newPrincipal
                            LastPaymentDate = time
                            MissedPayments = 0 }

                    [ MoneyTransferred(
                          characterAccount,
                          Outgoing(payment, balance - payment)
                      )
                      LoanPaid updatedLoan
                      NotificationShown(
                          Notification.LoanNotification(
                              LoanNotificationType.LoanSeasonalPaymentMade
                                  payment
                          )
                      ) ]
            else
                let newMissed = loan.MissedPayments + 1

                if newMissed = 1 then
                    let updatedLoan = { loan with MissedPayments = newMissed }

                    [ LoanPaymentMissed(updatedLoan, Bank.reputation state)
                      NotificationShown(
                          Notification.LoanNotification
                              LoanNotificationType.LoanPaymentMissedWarning
                      ) ]
                else
                    let fee =
                        loan.Principal * decimal Config.Loan.lateFeeRate

                    let newPrincipal = loan.Principal + fee

                    let currentReputation = Bank.reputation state

                    let newReputation =
                        match currentReputation with
                        | GoodStanding -> Flagged
                        | Flagged -> Blacklisted
                        | Blacklisted -> Blacklisted

                    let updatedLoan =
                        { loan with
                            Principal = newPrincipal
                            MissedPayments = newMissed
                            InterestRate =
                                Bank.interestRateForReputation newReputation }

                    [ LoanPaymentMissed(updatedLoan, newReputation)
                      BankReputationChanged(
                          Diff(currentReputation, newReputation)
                      )
                      NotificationShown(
                          Notification.LoanNotification(
                              LoanNotificationType.LoanPaymentMissedWithFee fee
                          )
                      ) ]

module Duets.Simulation.Bank.Operations

open Duets.Simulation.Queries
open Duets.Entities

type TransactionError = NotEnoughFunds of amount: Amount

let private withBalanceChecking state sender amount effects =
    let accountBalance = Bank.balanceOf state sender

    if amount <= accountBalance then
        Ok effects
    else
        Error(NotEnoughFunds amount)

/// Transfers the given amount from `from` to `to` if possible, returning whether
/// the transfer was successful or if there was some error on the way.
let transfer state sender receiver amount =
    withBalanceChecking
        state
        sender
        amount
        [ MoneyTransferred
          <| (receiver, Incoming(amount, Bank.balanceOf state receiver + amount))
          MoneyTransferred
          <| (sender, Outgoing(amount, Bank.balanceOf state sender - amount)) ]

/// Creates an outgoing transaction from the sender if possible, returning whether
/// the transfer was successful or if there was some error.
let expense state sender amount =
    withBalanceChecking
        state
        sender
        amount
        [ MoneyTransferred
          <| (sender, Outgoing(amount, Bank.balanceOf state sender - amount)) ]

/// Generates an income transaction to the given bank account.
let income state receiver amount =
    MoneyEarned(
        receiver,
        Incoming(amount, Bank.balanceOf state receiver + amount)
    )

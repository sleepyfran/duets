module Simulation.Bank.Transfer

open Simulation.Queries
open Entities

type TransactionResult =
    | Ok of Effect list
    | NotEnoughFunds

/// Transfers the given amount from `from` to `to` if possible, returning whether
/// the transfer was successful or there was some error on the way.
let transfer state sender receiver amount =
    let accountBalance = Bank.balanceOf state sender

    if accountBalance < amount then
        NotEnoughFunds
    else
        Ok [ MoneyTransferred <| (receiver, Incoming amount)
             MoneyTransferred <| (sender, Outgoing amount) ]

module Simulation.Tests.Bank.Transfer

open NUnit.Framework
open FsUnit
open Test.Common

open Common
open Entities
open Simulation.Bank.Operations

[<Test>]
let transferAmountHigherThanBalanceShouldReturnNotEnoughFunds () =
    [ 10101<dd>
      10120<dd>
      12000<dd>
      15000<dd> ]
    |> List.iter
        (fun amount ->
            transfer
                dummyState
                dummyCharacterBankAccount.Holder
                dummyBandBankAccount.Holder
                amount
            |> should be (ofCase <@ NotEnoughFunds @>))

[<Test>]
let transferAmountLowerThanBalanceShouldReturnOkWithEffects () =
    [ 200<dd>
      300<dd>
      990<dd>
      10000<dd>
      10<dd>
      540<dd> ]
    |> List.iter
        (fun amount ->
            transfer
                dummyState
                dummyCharacterBankAccount.Holder
                dummyBandBankAccount.Holder
                amount
            |> Result.unwrap
            |> should
                equal
                [ MoneyTransferred
                  <| (dummyBandBankAccount.Holder, Incoming(amount, amount))
                  MoneyTransferred
                  <| (dummyCharacterBankAccount.Holder,
                      Outgoing(amount, 10000<dd> - amount)) ])

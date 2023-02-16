module Duets.Simulation.Tests.Bank.Transfer

open NUnit.Framework
open FsUnit
open Test.Common

open Duets.Common
open Duets.Entities
open Duets.Simulation.Bank.Operations

[<Test>]
let transferAmountHigherThanBalanceShouldReturnNotEnoughFunds () =
    [ 10101m<dd>
      10120m<dd>
      12000m<dd>
      15000m<dd> ]
    |> List.iter (fun amount ->
        transfer
            dummyState
            dummyCharacterBankAccount.Holder
            dummyBandBankAccount.Holder
            amount
        |> should be (ofCase <@ NotEnoughFunds @>))

[<Test>]
let transferAmountLowerThanBalanceShouldReturnOkWithEffects () =
    [ 200m<dd>
      300m<dd>
      990m<dd>
      10m<dd>
      540m<dd> ]
    |> List.iter (fun amount ->
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
                  Outgoing(amount, 1000m<dd> - amount)) ])

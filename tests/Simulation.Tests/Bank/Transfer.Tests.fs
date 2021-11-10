module Simulation.Tests.Bank.Transfer

open NUnit.Framework
open FsUnit
open Test.Common

open Entities
open Simulation.Bank.Transfer

let state =
    dummyState
    |> addFunds dummyCharacterBankAccount.Holder 100<dd>

[<Test>]
let transferAmountHigherThanBalanceShouldReturnNotEnoughFunds () =
    [ 10101<dd>
      10120<dd>
      12000<dd>
      15000<dd> ]
    |> List.iter
        (fun amount ->
            transfer
                state
                dummyCharacterBankAccount.Holder
                dummyBandBankAccount.Holder
                amount
            |> should be (ofCase <@ NotEnoughFunds @>))

let createOk sender receiver amount =
    Ok
    <| [ MoneyTransferred <| (receiver, Incoming amount)
         MoneyTransferred <| (sender, Outgoing amount) ]

[<Test>]
let transferAmountLowerThanBalanceShouldReturnOkWithEffects () =
    [ 20<dd>
      30<dd>
      99<dd>
      1<dd>
      54<dd> ]
    |> List.iter
        (fun amount ->
            transfer
                state
                dummyCharacterBankAccount.Holder
                dummyBandBankAccount.Holder
                amount
            |> should
                equal
                (createOk
                    dummyCharacterBankAccount.Holder
                    dummyBandBankAccount.Holder
                    amount))

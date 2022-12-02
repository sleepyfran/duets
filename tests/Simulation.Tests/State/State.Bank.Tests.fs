module Simulation.Tests.State.Bank



open FsUnit
open NUnit.Framework
open Test.Common

open Aether
open Entities
open Simulation

let testBalance createTransfer expectedBalance =
    let holder =
        dummyCharacterBankAccount.Holder

    let state =
        createTransfer holder
        |> State.Root.applyEffect dummyState

    let balanceLenses =
        Lenses.FromState.BankAccount.balanceOf_ holder

    let balance =
        Optic.get balanceLenses state
        |> Option.defaultValue 0m<dd>

    balance |> should equal expectedBalance

[<Test>]
let ``MoneyTransferred should set balance to the account when transfer is incoming``
    ()
    =
    testBalance
        (fun holder -> MoneyTransferred(holder, Incoming(50m<dd>, 100m<dd>)))
        100m<dd>

[<Test>]
let ``MoneyTransferred should set balance to the account when transfer is outgoing``
    ()
    =
    testBalance
        (fun holder -> MoneyTransferred(holder, Outgoing(50m<dd>, 0m<dd>)))
        0m<dd>

[<Test>]
let ``MoneyEarned sets balance to the account when transfer is incoming`` () =
    testBalance
        (fun holder -> MoneyEarned(holder, Incoming(50m<dd>, 50m<dd>)))
        50m<dd>

[<Test>]
let ``MoneyEarned sets balance to the account when transfer is outgoing (not that it should happen, but anyway.)``
    ()
    =
    testBalance
        (fun holder -> MoneyEarned(holder, Outgoing(50m<dd>, 0m<dd>)))
        0m<dd>

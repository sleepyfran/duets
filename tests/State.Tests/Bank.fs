module State.Tests.Bank

open FsUnit
open NUnit.Framework
open Test.Common

open Aether
open Common
open Entities

[<SetUp>]
let Setup () = initState ()

[<Test>]
let TransferShouldAddTransactionToBankAccount () =
    let holder = dummyCharacterBankAccount.Holder

    State.Root.apply
    <| MoneyTransferred(holder, Incoming 100<dd>)

    State.Root.apply
    <| MoneyTransferred(holder, Outgoing 50<dd>)

    let state = State.Root.get ()

    let transactionLenses =
        Lenses.FromState.BankAccount.transactionsOf_ holder

    let transactions =
        Optic.get transactionLenses state
        |> Option.defaultValue []

    transactions |> should haveLength 2

    List.head transactions
    |> should equal (Outgoing 50<dd>)

    List.item 1 transactions
    |> should equal (Incoming 100<dd>)

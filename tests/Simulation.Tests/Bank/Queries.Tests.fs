module Simulation.Tests.Bank.Queries

open NUnit.Framework
open FsUnit
open Test.Common

open Entities
open Simulation.Queries.Bank

[<Test>]
let balanceOfShouldReturnCorrectBalance () =
    let state =
        dummyState
        |> addFunds dummyCharacterBankAccount.Holder 100<dd>

    balanceOf state dummyCharacterBankAccount.Holder
    |> should equal 100

[<Test>]
let balanceOfUnknownShouldReturn0 () =
    balanceOf dummyState (Character(CharacterId <| Identity.create ()))
    |> should equal 0

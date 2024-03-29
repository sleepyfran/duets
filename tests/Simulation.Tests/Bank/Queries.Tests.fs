module Duets.Simulation.Tests.Bank.Queries

open NUnit.Framework
open FsUnit
open Test.Common

open Duets.Entities
open Duets.Simulation.Queries.Bank

[<Test>]
let balanceOfShouldReturnCorrectBalance () =
    balanceOf dummyState dummyCharacterBankAccount.Holder
    |> should equal 1000

[<Test>]
let balanceOfUnknownShouldReturn0 () =
    balanceOf dummyState (Character(CharacterId <| Identity.create ()))
    |> should equal 0

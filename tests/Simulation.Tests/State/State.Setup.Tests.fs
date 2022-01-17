module Simulation.State.Tests.Setup


open FsUnit
open NUnit.Framework
open Test.Common
open Entities
open Simulation

[<Test>]
let GameCreatedShouldInitializeState () =
    GameCreated dummyState
    |> State.Root.applyEffect dummyState
    |> should equal dummyState

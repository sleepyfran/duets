module Duets.Simulation.Tests.State.Setup

open FsUnit
open NUnit.Framework
open Test.Common
open Duets.Entities
open Duets.Simulation

[<Test>]
let GameCreatedShouldInitializeState () =
    GameCreated dummyState
    |> State.Root.applyEffect dummyState
    |> should equal dummyState

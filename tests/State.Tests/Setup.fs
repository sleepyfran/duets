module State.Tests.Setup

open FsUnit
open NUnit.Framework
open Test.Common

open Entities

[<Test>]
let GameCreatedShouldInitializeState () =
    GameCreated dummyState |> State.Root.apply

    State.Root.get () |> should equal dummyState

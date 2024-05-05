module Duets.Simulation.Tests.Bands.SwitchGenre

open Test.Common
open NUnit.Framework
open FsUnit

open Duets.Entities

[<Test>]
let ``switchGenre returns an error if genre is the same as current`` () =
    let currentGenre = dummyBand.Genre

    RehearsalRoomSwitchToGenre
        {| Band = dummyBand
           Genre = currentGenre |}
    |> runFailingAction dummyState
    |> should equal (BandAlreadyHasGenre currentGenre)

[<Test>]
let ``switchGenre returns BandSwitchedGenre when switch is possible`` () =
    RehearsalRoomSwitchToGenre
        {| Band = dummyBand
           Genre = "Black Metal" |}
    |> runSucceedingAction dummyState
    |> fst
    |> should
        contain
        (BandSwitchedGenre(dummyBand, Diff(dummyBand.Genre, "Black Metal")))

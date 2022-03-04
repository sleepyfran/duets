module Simulation.Tests.State.Concerts

open FsUnit
open NUnit.Framework
open Test.Common
open Test.Common.Generators

open Entities
open Simulation
open Simulation.Queries

let private stateGenOptions =
    { State.defaultOptions with
          FutureConcertsToGenerate = 0 }

[<Test>]
let ``ConcertScheduled adds a concert to the given band's future schedule`` () =
    let state = State.generateOne stateGenOptions

    let state =
        ConcertScheduled(dummyBand, ScheduledConcert dummyConcert)
        |> State.Root.applyEffect state

    Concerts.allScheduled state dummyBand.Id
    |> should haveCount 1

[<Test>]
let ``ConcertUpdated removes the previous concert and adds it again with the new data``
    ()
    =
    let state = State.generateOne stateGenOptions

    let state =
        ConcertScheduled(dummyBand, ScheduledConcert dummyConcert)
        |> State.Root.applyEffect state

    let state =
        ConcertUpdated(
            dummyBand,
            ScheduledConcert { dummyConcert with TicketsSold = 250 }
        )
        |> State.Root.applyEffect state

    let concerts =
        Concerts.allScheduled state dummyBand.Id
        |> Set.map Concert.fromScheduled

    concerts |> should haveCount 1

    concerts
    |> Set.iter (fun concert -> concert.TicketsSold |> should equal 250)

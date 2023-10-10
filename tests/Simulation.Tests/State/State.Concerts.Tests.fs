module Duets.Simulation.Tests.State.Concerts

open FsUnit
open NUnit.Framework
open Test.Common
open Test.Common.Generators

open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Queries

let private stateGenOptions =
    { State.defaultOptions with
        FutureConcertsToGenerate = 0 }

[<Test>]
let ``ConcertScheduled adds a concert to the given band's future schedule`` () =
    let state = State.generateOne stateGenOptions

    let state =
        ConcertScheduled(dummyBand, ScheduledConcert(dummyConcert, dummyToday))
        |> State.Root.applyEffect state

    Concerts.allScheduled state dummyBand.Id |> should haveLength 1

[<Test>]
let ``ConcertUpdated removes the previous concert and adds it again with the new data``
    ()
    =
    let state = State.generateOne stateGenOptions

    let state =
        ConcertScheduled(dummyBand, ScheduledConcert(dummyConcert, dummyToday))
        |> State.Root.applyEffect state

    let state =
        ConcertUpdated(
            dummyBand,
            ScheduledConcert(
                { dummyConcert with TicketsSold = 250 },
                dummyToday
            )
        )
        |> State.Root.applyEffect state

    let concerts =
        Concerts.allScheduled state dummyBand.Id
        |> List.map Concert.fromScheduled

    concerts |> should haveLength 1

    concerts
    |> List.iter (fun concert -> concert.TicketsSold |> should equal 250)

[<Test>]
let ``ConcertCancelled removes scheduled concert and adds it as failed to past concerts``
    ()
    =
    let state = State.generateOne stateGenOptions

    let state =
        ConcertCancelled(
            dummyBand,
            FailedConcert(dummyConcert, BandDidNotMakeIt)
        )
        |> State.Root.applyEffect state

    let scheduledConcerts = Concerts.allScheduled state dummyBand.Id

    let pastConcerts = Concerts.allPast state dummyBand.Id

    scheduledConcerts |> should haveLength 0
    pastConcerts |> should haveLength 1

    pastConcerts
    |> List.iter (fun concert ->
        concert |> should be (ofCase <@ FailedConcert @>))

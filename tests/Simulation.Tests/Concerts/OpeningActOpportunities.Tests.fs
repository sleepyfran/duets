module Duets.Simulation.Tests.Concerts.OpeningActOpportunities

open Duets.Common
open Duets.Entities
open Duets.Simulation

open Duets.Simulation.Concerts
open FsUnit
open NUnit.Framework
open Test.Common
open Test.Common.Generators

[<Test>]
let ``applyToConcertOpportunity returns NotEnoughFame if headliner fame is more than 25 of the band's fame``
    ()
    =
    let state =
        State.generateOne
            { State.defaultOptions with
                BandFansMin = 1
                BandFansMax = 1 }

    OpeningActOpportunities.applyToConcertOpportunity
        state
        dummyHeadlinerBand
        dummyConcert
    |> Result.unwrapError
    |> should be (ofCase <@ OpeningActOpportunities.NotEnoughFame @>)

[<Test>]
let ``applyToConcertOpportunity returns NotEnoughReleases if band does not have anything released yet``
    ()
    =
    let state =
        State.generateOne
            { State.defaultOptions with
                BandFansMin = 20000
                BandFansMax = 30000 }

    OpeningActOpportunities.applyToConcertOpportunity
        state
        dummyHeadlinerBand
        dummyConcert
    |> Result.unwrapError
    |> should be (ofCase <@ OpeningActOpportunities.NotEnoughReleases @>)

[<Test>]
let ``applyToConcertOpportunity returns AnotherConcertAlreadyScheduled if band already has a concert scheduled on that date``
    ()
    =
    let state =
        State.generateOne
            { State.defaultOptions with
                BandFansMin = 20000
                BandFansMax = 30000 }
        |> State.Albums.addReleased dummyBand dummyReleasedAlbum
        |> State.Concerts.addScheduledConcert
            dummyBand
            (ScheduledConcert(dummyConcert, dummyToday))

    OpeningActOpportunities.applyToConcertOpportunity
        state
        dummyHeadlinerBand
        dummyConcert
    |> Result.unwrapError
    |> should
        be
        (ofCase <@ OpeningActOpportunities.AnotherConcertAlreadyScheduled @>)

[<Test>]
let ``applyToConcertOpportunity returns ok with effects if all checks succeed``
    ()
    =
    let state =
        State.generateOne
            { State.defaultOptions with
                BandFansMin = 20000
                BandFansMax = 30000 }
        |> State.Albums.addReleased dummyBand dummyReleasedAlbum

    OpeningActOpportunities.applyToConcertOpportunity
        state
        dummyHeadlinerBand
        dummyConcert
    |> Result.unwrap
    |> should be (ofCase <@ ConcertScheduled @>)

[<Test>]
let ``applyToConcertOpportunity returns ok if band fame is higher than headliner``
    ()
    =
    let state =
        State.generateOne
            { State.defaultOptions with
                BandFansMin = 2000000
                BandFansMax = 3000000 }
        |> State.Albums.addReleased dummyBand dummyReleasedAlbum

    OpeningActOpportunities.applyToConcertOpportunity
        state
        dummyHeadlinerBand
        dummyConcert
    |> Result.unwrap
    |> should be (ofCase <@ ConcertScheduled @>)

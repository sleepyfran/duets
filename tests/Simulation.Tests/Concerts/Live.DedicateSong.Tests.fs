module Simulation.Tests.Concerts.Live.DedicateSong

open FsCheck
open FsUnit
open NUnit.Framework
open Test.Common

open Aether
open Entities
open Simulation.Concerts.Live

[<Test>]
let ``dedicateSong adds 10 points on top of the result of play song`` () =
    Generators.Song.finishedGenerator Generators.Song.defaultOptions
    |> Gen.sample 0 1000
    |> List.iter
        (fun song ->
            let response =
                dedicateSong dummyOngoingConcert song Energetic

            response
            |> ongoingConcertFromResponse
            |> Optic.get Lenses.Concerts.Ongoing.points_
            |> should be (inRange 10<quality> 25<quality>)

            response
            |> pointsFromResponse
            |> should be (inRange 10<quality> 25<quality>))

[<Test>]
let ``dedicateSong adds a dedicated song event to the event list`` () =
    let response =
        dedicateSong dummyOngoingConcert dummyFinishedSong Energetic

    response.OngoingConcert.Events
    |> should contain (CommonEvent DedicateSong)

[<Test>]
let ``dedicateSong returns result from play song`` () =
    let response =
        dedicateSong dummyOngoingConcert dummyFinishedSong Energetic

    response.Result
    |> should be (ofCase <@ Dedicated @>)

[<Test>]
let ``dedicateSong returns TooManyDedications if more than two songs were dedicated``
    ()
    =
    let response =
        dedicateSong dummyOngoingConcert dummyFinishedSong Energetic
        |> fun res ->
            dedicateSong res.OngoingConcert dummyFinishedSong Energetic
        |> fun res ->
            dedicateSong res.OngoingConcert dummyFinishedSong Energetic

    response.Result
    |> should be (ofCase <@ TooManyDedications @>)

module Duets.Simulation.Tests.Concerts.Live.DedicateSong

open FsCheck
open FsUnit
open NUnit.Framework
open Test.Common

open Aether
open Duets.Entities
open Duets.Simulation.Concerts.Live

[<Test>]
let ``dedicateSong adds 10 points on top of the result of play song`` () =
    Generators.Song.finishedGenerator Generators.Song.defaultOptions
    |> Gen.sample 0 1000
    |> List.iter (fun song ->
        let response =
            dedicateSong dummyState dummyOngoingConcert song Energetic

        response
        |> ongoingConcertFromEffectList
        |> Optic.get Lenses.Concerts.Ongoing.points_
        |> should be (inRange 10<quality> 25<quality>)

        response
        |> pointsFromEffectList
        |> should be (inRange 10<quality> 25<quality>))

[<Test>]
let ``dedicateSong adds a dedicated song event to the event list`` () =
    let ongoingConcert =
        dedicateSong dummyState dummyOngoingConcert dummyFinishedSong Energetic
        |> ongoingConcertFromEffectList

    ongoingConcert.Events
    |> List.filter (function
        | DedicateSong _ -> true
        | _ -> false)
    |> should haveLength 1

[<Test>]
let ``dedicateSong returns result from play song`` () =
    let result =
        dedicateSong dummyState dummyOngoingConcert dummyFinishedSong Energetic
        |> resultFromEffectList

    result |> should be (ofCase <@ AveragePerformance @>)

[<Test>]
let ``dedicateSong returns TooManyDedications if more than two songs were dedicated``
    ()
    =
    let result =
        dedicateSong dummyState dummyOngoingConcert dummyFinishedSong Energetic
        |> fun res ->
            let ongoingConcert = ongoingConcertFromEffectList res

            dedicateSong dummyState ongoingConcert dummyFinishedSong Energetic
        |> fun res ->
            let ongoingConcert = ongoingConcertFromEffectList res

            dedicateSong dummyState ongoingConcert dummyFinishedSong Energetic
        |> resultFromEffectList

    result |> should be (ofCase <@ TooManyRepetitionsPenalized @>)

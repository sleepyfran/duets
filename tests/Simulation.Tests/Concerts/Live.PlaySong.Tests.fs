module Simulation.Tests.Concerts.Live.PlaySong

open FsCheck
open FsUnit
open NUnit.Framework
open Test.Common

open Aether
open Entities
open Simulation.Concerts.Live

[<Test>]
let ``playSong energetic energy gives up to 15 points`` () =
    Generators.Song.finishedGenerator Generators.Song.defaultOptions
    |> Gen.sample 0 1000
    |> List.iter
        (fun song ->
            let response =
                playSong dummyState dummyOngoingConcert song Energetic

            response
            |> ongoingConcertFromResponse
            |> Optic.get Lenses.Concerts.Ongoing.points_
            |> should be (inRange 0<quality> 15<quality>)

            response
            |> pointsFromResponse
            |> should be (inRange 0<quality> 15<quality>))

[<Test>]
let ``playSong normal energy gives up to 8 points`` () =
    Generators.Song.finishedGenerator Generators.Song.defaultOptions
    |> Gen.sample 0 1000
    |> List.iter
        (fun song ->
            let response =
                playSong
                    dummyState
                    dummyOngoingConcert
                    song
                    PerformEnergy.Normal

            response
            |> ongoingConcertFromResponse
            |> Optic.get Lenses.Concerts.Ongoing.points_
            |> should be (inRange 0<quality> 8<quality>)

            response
            |> pointsFromResponse
            |> should be (inRange 0<quality> 8<quality>))

[<Test>]
let ``playSong limited energy gives up to 2 points`` () =
    Generators.Song.finishedGenerator Generators.Song.defaultOptions
    |> Gen.sample 0 1000
    |> List.iter
        (fun song ->
            let response =
                playSong dummyState dummyOngoingConcert song Limited

            response
            |> ongoingConcertFromResponse
            |> Optic.get Lenses.Concerts.Ongoing.points_
            |> should be (inRange 0<quality> 2<quality>)

            response
            |> pointsFromResponse
            |> should be (inRange 0<quality> 2<quality>))

[<Test>]
let ``playSong decreases points by 50 if song has been played already`` () =
    let finishedSong =
        Generators.Song.finishedGenerator Generators.Song.defaultOptions
        |> Gen.sample 0 1
        |> List.head

    let FinishedSong song, _ = finishedSong

    let ongoingConcert =
        { dummyOngoingConcert with
              Events = [ PlaySong(song, Energetic) ]
              Points = 50<quality> }

    playSong dummyState ongoingConcert finishedSong Energetic
    |> ongoingConcertFromResponse
    |> Optic.get Lenses.Concerts.Ongoing.points_
    |> should equal 0

[<Test>]
let ``playSong returns low performance result if practice is below 25`` () =
    Generators.Song.finishedGenerator { PracticeMin = 0; PracticeMax = 24 }
    |> Gen.sample 0 1000
    |> List.iter
        (fun song ->
            let response =
                playSong dummyState dummyOngoingConcert song Energetic

            response
            |> resultFromResponse
            |> should be (ofCase <@ LowPerformance @>))

[<Test>]
let ``playSong returns average performance result if practice is between 25 50``
    ()
    =
    Generators.Song.finishedGenerator { PracticeMin = 25; PracticeMax = 49 }
    |> Gen.sample 0 1000
    |> List.iter
        (fun song ->
            let response =
                playSong dummyState dummyOngoingConcert song Energetic

            response
            |> resultFromResponse
            |> should be (ofCase <@ AveragePerformance @>))

[<Test>]
let ``playSong returns good performance result if practice is below 75`` () =
    Generators.Song.finishedGenerator { PracticeMin = 50; PracticeMax = 74 }
    |> Gen.sample 0 1000
    |> List.iter
        (fun song ->
            let response =
                playSong dummyState dummyOngoingConcert song Energetic

            response
            |> resultFromResponse
            |> should be (ofCase <@ GoodPerformance @>))

[<Test>]
let ``playSong returns great performance result if practice is above 75`` () =
    Generators.Song.finishedGenerator { PracticeMin = 76; PracticeMax = 100 }
    |> Gen.sample 0 1000
    |> List.iter
        (fun song ->
            let response =
                playSong dummyState dummyOngoingConcert song Energetic

            response
            |> resultFromResponse
            |> should be (ofCase <@ GreatPerformance @>))

[<Test>]
let ``playSong does not decrease points below 0`` () =
    let finishedSong =
        Generators.Song.finishedGenerator Generators.Song.defaultOptions
        |> Gen.sample 0 1
        |> List.head

    let ongoingConcert =
        { dummyOngoingConcert with
              Events = [ PlaySong(Song.fromFinished finishedSong, Energetic) ]
              Points = 20<quality> }

    playSong dummyState ongoingConcert finishedSong Energetic
    |> ongoingConcertFromResponse
    |> Optic.get Lenses.Concerts.Ongoing.points_
    |> should equal 0

[<Test>]
let ``playSong should add points to the previous count to ongoing concert`` () =
    let ongoingConcert =
        { dummyOngoingConcert with
              Points = 50<quality> }

    Generators.Song.finishedGenerator
        { Generators.Song.defaultOptions with
              PracticeMin = 1 }
    |> Gen.sample 0 1000
    |> List.iter
        (fun song ->
            playSong dummyState ongoingConcert song Energetic
            |> ongoingConcertFromResponse
            |> Optic.get Lenses.Concerts.Ongoing.points_
            |> should be (greaterThan 50<quality>))

[<Test>]
let ``playSong does not increase above 100`` () =
    let ongoingConcert =
        { dummyOngoingConcert with
              Points = 98<quality> }

    Generators.Song.finishedGenerator Generators.Song.defaultOptions
    |> Gen.sample 0 1000
    |> List.iter
        (fun song ->
            playSong dummyState ongoingConcert song Energetic
            |> ongoingConcertFromResponse
            |> Optic.get Lenses.Concerts.Ongoing.points_
            |> should be (lessThanOrEqualTo 100<quality>))

[<Test>]
let ``playSong should add event when the song hasn't been played before`` () =
    Generators.Song.finishedGenerator Generators.Song.defaultOptions
    |> Gen.sample 0 1
    |> List.head
    |> fun song ->
        playSong dummyState dummyOngoingConcert song Energetic
        |> ongoingConcertFromResponse
        |> Optic.get Lenses.Concerts.Ongoing.events_
        |> should contain (PlaySong(Song.fromFinished song, Energetic))

[<Test>]
let ``playSong should add event when the song was played before`` () =
    Generators.Song.finishedGenerator Generators.Song.defaultOptions
    |> Gen.sample 0 1
    |> List.head
    |> fun finishedSong ->
        let song = Song.fromFinished finishedSong

        let ongoingConcert =
            { dummyOngoingConcert with
                  Events = [ PlaySong(song, Energetic) ]
                  Points = 40<quality> }

        playSong dummyState ongoingConcert finishedSong Energetic
        |> ongoingConcertFromResponse
        |> Optic.get Lenses.Concerts.Ongoing.events_
        |> List.filter (fun event -> event = (PlaySong(song, Energetic)))
        |> should haveLength 2

module Simulation.Tests.Concerts.Live.PlaySong


open FsCheck
open FsUnit
open NUnit.Framework
open Test.Common

open Aether
open Entities
open Simulation.Concerts.Live

let private ongoingConcert = { Events = []; Points = 0<quality> }

let private ongoingConcertFromResponse response = response.OngoingConcert

[<Test>]
let ``playSong energetic energy gives up to 15 points`` () =
    Generators.Song.finishedGenerator Generators.Song.defaultOptions
    |> Gen.sample 0 1000
    |> List.iter (fun song ->
        playSong ongoingConcert song Energetic
        |> ongoingConcertFromResponse
        |> Optic.get Lenses.Concerts.Ongoing.points_
        |> should be (inRange 0<quality> 15<quality>))

[<Test>]
let ``playSong normal energy gives up to 8 points`` () =
    Generators.Song.finishedGenerator Generators.Song.defaultOptions
    |> Gen.sample 0 1000
    |> List.iter (fun song ->
        playSong ongoingConcert song PerformEnergy.Normal
        |> ongoingConcertFromResponse
        |> Optic.get Lenses.Concerts.Ongoing.points_
        |> should be (inRange 0<quality> 8<quality>))

[<Test>]
let ``playSong limited energy gives up to 2 points`` () =
    Generators.Song.finishedGenerator Generators.Song.defaultOptions
    |> Gen.sample 0 1000
    |> List.iter (fun song ->
        playSong ongoingConcert song Limited
        |> ongoingConcertFromResponse
        |> Optic.get Lenses.Concerts.Ongoing.points_
        |> should be (inRange 0<quality> 2<quality>))

[<Test>]
let ``playSong decreases points by 50 if song has been played already`` () =
    let finishedSong =
        Generators.Song.finishedGenerator Generators.Song.defaultOptions
        |> Gen.sample 0 1
        |> List.head

    let FinishedSong song, _ = finishedSong

    let ongoingConcert =
        { ongoingConcert with
            Events = [ PlaySong(song, Energetic) |> CommonEvent ]
            Points = 50<quality> }

    playSong ongoingConcert finishedSong Energetic
    |> ongoingConcertFromResponse
    |> Optic.get Lenses.Concerts.Ongoing.points_
    |> should equal 0

[<Test>]
let ``playSong does not decrease points below 0`` () =
    let finishedSong =
        Generators.Song.finishedGenerator Generators.Song.defaultOptions
        |> Gen.sample 0 1
        |> List.head

    let ongoingConcert =
        { ongoingConcert with
            Events =
                [ PlaySong(Song.fromFinished finishedSong, Energetic)
                  |> CommonEvent ]
            Points = 20<quality> }

    playSong ongoingConcert finishedSong Energetic
    |> ongoingConcertFromResponse
    |> Optic.get Lenses.Concerts.Ongoing.points_
    |> should equal 0

[<Test>]
let ``playSong should add points to the previous count`` () =
    let ongoingConcert = { ongoingConcert with Points = 50<quality> }

    Generators.Song.finishedGenerator
        { Generators.Song.defaultOptions with PracticeMin = 1 }
    |> Gen.sample 0 1000
    |> List.iter (fun song ->
        playSong ongoingConcert song Energetic
        |> ongoingConcertFromResponse
        |> Optic.get Lenses.Concerts.Ongoing.points_
        |> should be (greaterThan 50<quality>))

[<Test>]
let ``playSong does not increase above 100`` () =
    let ongoingConcert = { ongoingConcert with Points = 98<quality> }

    Generators.Song.finishedGenerator Generators.Song.defaultOptions
    |> Gen.sample 0 1000
    |> List.iter (fun song ->
        playSong ongoingConcert song Energetic
        |> ongoingConcertFromResponse
        |> Optic.get Lenses.Concerts.Ongoing.points_
        |> should be (lessThanOrEqualTo 100<quality>))

[<Test>]
let ``playSong should add event when the song hasn't been played before`` () =
    Generators.Song.finishedGenerator Generators.Song.defaultOptions
    |> Gen.sample 0 1
    |> List.head
    |> fun song ->
        playSong ongoingConcert song Energetic
        |> ongoingConcertFromResponse
        |> Optic.get Lenses.Concerts.Ongoing.events_
        |> should
            contain
            (PlaySong(Song.fromFinished song, Energetic)
             |> CommonEvent)

[<Test>]
let ``playSong should add event when the song was played before`` () =
    Generators.Song.finishedGenerator Generators.Song.defaultOptions
    |> Gen.sample 0 1
    |> List.head
    |> fun finishedSong ->
        let song = Song.fromFinished finishedSong

        let ongoingConcert =
            { ongoingConcert with
                Events = [ PlaySong(song, Energetic) |> CommonEvent ]
                Points = 40<quality> }

        playSong ongoingConcert finishedSong Energetic
        |> ongoingConcertFromResponse
        |> Optic.get Lenses.Concerts.Ongoing.events_
        |> List.filter (fun event ->
            event = (PlaySong(song, Energetic) |> CommonEvent))
        |> should haveLength 2

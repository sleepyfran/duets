module Duets.Simulation.Tests.Events.Band

open FsCheck
open FsUnit
open NUnit.Framework
open Test.Common
open Test.Common.Generators

open Duets.Entities
open Duets.Simulation

let bandFansChanged fansBefore fansAfter =
    BandFansChanged(dummyBand, Diff(fansBefore, fansAfter))

let stateWithAlbum =
    State.generateOne State.defaultOptions
    |> State.Albums.addReleased dummyBand dummyReleasedAlbum

let filterReviewsReceived =
    List.filter (function
        | AlbumReviewsReceived _ -> true
        | _ -> false)

[<Test>]
let ``tick of band fans changed should not generate reviews if count hasn't gotten past the minimum``
    ()
    =
    Gen.choose (0, Config.MusicSimulation.minimumFanBaseForReviews - 2)
    |> Gen.sample 0 100
    |> List.iter (fun previousFans ->
        Simulation.tickOne
            stateWithAlbum
            (bandFansChanged previousFans (previousFans + 1))
        |> fst
        |> filterReviewsReceived
        |> should haveLength 0)

[<Test>]
let ``tick of band fans changed should not generate reviews if count has already gotten past the minimum before``
    ()
    =
    Gen.choose (
        Config.MusicSimulation.minimumFanBaseForReviews + 1,
        Config.MusicSimulation.minimumFanBaseForReviews + 10000
    )
    |> Gen.sample 0 100
    |> List.iter (fun previousFans ->
        Simulation.tickOne
            stateWithAlbum
            (bandFansChanged previousFans (previousFans + 1))
        |> fst
        |> filterReviewsReceived
        |> should haveLength 0)

[<Test>]
let ``tick of band fans changed should generate reviews if count has just gotten past the minimum``
    ()
    =
    Gen.choose (
        Config.MusicSimulation.minimumFanBaseForReviews + 1,
        Config.MusicSimulation.minimumFanBaseForReviews + 10000
    )
    |> Gen.sample 0 100
    |> List.iter (fun updatedFans ->
        Simulation.tickOne
            stateWithAlbum
            (bandFansChanged
                (Config.MusicSimulation.minimumFanBaseForReviews - 1)
                updatedFans)
        |> fst
        |> filterReviewsReceived
        |> should haveLength 1)

[<Test>]
let ``tick of band fans changed should generate reviews for all previously released albums``
    ()
    =
    let secondUnreleasedAlbum =
        Album.Unreleased.from dummyBand "Test Album 2" dummyRecordedSongRef

    let secondAlbum =
        Album.Released.fromUnreleased secondUnreleasedAlbum dummyToday 1.0

    let thirdUnreleasedAlbum =
        Album.Unreleased.from dummyBand "Test Album 3" dummyRecordedSongRef

    let thirdAlbum =
        Album.Released.fromUnreleased thirdUnreleasedAlbum dummyToday 1.0

    let state =
        stateWithAlbum
        |> State.Albums.addReleased dummyBand secondAlbum
        |> State.Albums.addReleased dummyBand thirdAlbum

    Simulation.tickOne
        state
        (bandFansChanged
            (Config.MusicSimulation.minimumFanBaseForReviews - 1)
            (Config.MusicSimulation.minimumFanBaseForReviews + 1))
    |> fst
    |> filterReviewsReceived
    |> should haveLength 3

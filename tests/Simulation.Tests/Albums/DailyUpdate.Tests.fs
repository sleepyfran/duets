module Duets.Simulation.Tests.Albums.DailyUpdate

open Aether
open Aether.Operators
open FsUnit
open NUnit.Framework
open Test.Common
open Test.Common.Generators

open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Albums.DailyUpdate

let album = Album.Released.fromUnreleased dummyUnreleasedAlbum dummyToday 1.0

let state =
    State.generateOne
        { State.defaultOptions with
            BandFansMin = 100<fans>
            BandFansMax = 1000<fans> }
    |> addReleasedAlbum dummyBand.Id album

[<Test>]
let ``dailyUpdate should return empty list if no albums released`` () =
    dailyUpdate dummyState |> should haveLength 0

[<Test>]
let ``dailyUpdate should sum new streams to the current count and update hype``
    ()
    =
    let streamsLenses =
        Lenses.FromState.Albums.releasedByBand_ dummyBand.Id
        >?> Map.key_ album.Album.Id
        >?> Lenses.Album.streams_

    let updatedState =
        dummyState
        |> addReleasedAlbum dummyBand.Id album
        |> Optic.set streamsLenses 1500

    dailyUpdate updatedState
    |> should
        contain
        (AlbumReleasedUpdate(dummyBand, Album.Released.update album 1501 0.9))

[<Test>]
let ``dailyUpdate should return list with money transfer if quantity is more than 0``
    ()
    =
    dailyUpdate state |> List.item 1 |> should be (ofCase <@ MoneyEarned @>)

[<Test>]
let ``dailyUpdate should return list without money transfer if quantity is 0``
    ()
    =
    let unknownAlbum =
        Album.Released.fromUnreleased dummyUnreleasedAlbum dummyToday 0

    let state = dummyState |> addReleasedAlbum dummyBand.Id unknownAlbum

    let updateEffects = dailyUpdate state
    updateEffects |> should haveLength 1

let private testDailyUpdateWith minFans maxFans maxFanDifference =
    State.generateN
        { State.defaultOptions with
            BandFansMin = minFans
            BandFansMax = maxFans }
        100
    |> List.iter (fun state ->
        let updateEffects =
            state |> addReleasedAlbum state.Bands.Current album |> dailyUpdate

        let effect =
            updateEffects
            |> List.tryItem 2
            |> Option.orElse (List.tryItem 1 updateEffects)
            |> Option.get

        let (Diff(_, updatedFanCount)) =
            match effect with
            | BandFansChanged(_, diff) -> diff
            | _ -> failwith "That's not the type we were expecting"

        let band = Queries.Bands.currentBand state
        let totalUpdatedFanCount = Queries.Bands.totalFans updatedFanCount
        let previousTotalFanCount = Queries.Bands.totalFans' band

        totalUpdatedFanCount - previousTotalFanCount
        |> should be (lessThanOrEqualTo maxFanDifference))

[<Test>]
let ``dailyUpdate should increase fans based on streams`` () =
    [ (0, 1000, 5)
      (1001, 15000, 25)
      (15001, 250000, 145)
      (250001, 1500000, 1450)
      (1500001, 10000000, 1500) ]
    |> List.iter (fun (minFans, maxFans, maxExpectedDifference) ->
        testDailyUpdateWith
            (minFans * 1<fans>)
            (maxFans * 1<fans>)
            maxExpectedDifference)

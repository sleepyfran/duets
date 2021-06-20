module Simulation.Tests.Galactus

open FsUnit
open NUnit.Framework
open Test.Common
open Fugit.Months

open Entities
open Simulation.Time.AdvanceTime
open Simulation.Galactus

let state =
    { dummyState with
          Today = dummyTodayMiddleOfYear }

let stateInMorning =
    { state with
          Today = state.Today |> Calendar.withDayMoment Morning }

let stateInMidnightBeforeGameStart =
    { state with
          Today =
              Calendar.gameBeginning
              |> Calendar.withDayMoment Midnight }

let unfinishedSong =
    (UnfinishedSong dummySong, 10<quality>, 10<quality>)

let songStartedEffect = SongStarted(dummyBand, unfinishedSong)

let effectsWithTimeIncrease =
    [ (songStartedEffect, 1)
      (SongImproved(dummyBand, Diff(unfinishedSong, unfinishedSong)), 1)
      (AlbumRecorded(dummyBand, dummyUnreleasedAlbum), 2) ]

let checkTimeIncrease timeIncrease effects =
    effects
    |> should
        contain
        (advanceDayMoment dummyTodayMiddleOfYear timeIncrease
         |> List.head)

[<Test>]
let ``runOne should advance time by corresponding effect type`` () =
    effectsWithTimeIncrease
    |> List.iter
        (fun (effect, timeIncrease) ->
            runOne state effect
            |> checkTimeIncrease timeIncrease)

let filterDailyUpdateEffects effect =
    match effect with
    | AlbumReleasedUpdate _ -> true
    | MoneyEarned _ -> true
    | _ -> false

[<Test>]
let ``runOne should update album streams every day`` () =
    let state =
        dummyState
        |> addReleasedAlbum
            dummyBand
            (Album.Released.fromUnreleased
                dummyUnreleasedAlbum
                dummyToday
                1500
                1.0)

    runOne state songStartedEffect
    |> List.filter filterDailyUpdateEffects
    |> should haveLength 2

[<Test>]
let ``runOne should not update album streams if morning has passed`` () =
    runOne stateInMorning songStartedEffect
    |> List.filter filterDailyUpdateEffects
    |> should haveLength 0

let filterMarketUpdateEffects effect =
    match effect with
    | GenreMarketsUpdated _ -> true
    | _ -> false

[<Test>]
let ``runOne should update markets every year in the dawn`` () =
    runOne stateInMidnightBeforeGameStart songStartedEffect
    |> List.filter filterMarketUpdateEffects
    |> should haveLength 1

    runOne dummyState songStartedEffect
    |> List.filter filterMarketUpdateEffects
    |> should haveLength 0

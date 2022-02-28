module Simulation.Tests.Simulation


open FsUnit
open NUnit.Framework
open Test.Common
open Test.Common.Generators

open Entities
open Simulation
open Simulation.Time.AdvanceTime

let state =
    { dummyState with
          Today = dummyTodayMiddleOfYear }

let stateInMorning =
    { state with
          Today =
              state.Today
              |> Calendar.Transform.changeDayMoment Morning }

let stateInMidnightBeforeGameStart =
    { state with
          Today =
              Calendar.gameBeginning
              |> Calendar.Transform.changeDayMoment Midnight }

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
let ``tick should advance time by corresponding effect type`` () =
    effectsWithTimeIncrease
    |> List.iter
        (fun (effect, timeIncrease) ->
            Simulation.tick state effect
            |> fst
            |> checkTimeIncrease timeIncrease)

let filterDailyUpdateEffects effect =
    match effect with
    | AlbumReleasedUpdate _ -> true
    | MoneyEarned _ -> true
    | ConcertUpdated _ -> true
    | _ -> false

[<Test>]
let ``tick should update album streams every day`` () =
    let state =
        dummyState
        |> addReleasedAlbum
            dummyBand
            (Album.Released.fromUnreleased
                dummyUnreleasedAlbum
                dummyToday
                1500
                1.0)

    Simulation.tick state songStartedEffect
    |> fst
    |> List.filter filterDailyUpdateEffects
    |> should haveLength 2

[<Test>]
let ``tick should update all scheduled concerts every day`` () =
    let state = State.generateOne State.defaultOptions

    Simulation.tick state songStartedEffect
    |> fst
    |> List.filter filterDailyUpdateEffects
    |> should haveLength 10

[<Test>]
let ``tick should not update album streams or concerts if morning has passed``
    ()
    =
    Simulation.tick stateInMorning songStartedEffect
    |> fst
    |> List.filter filterDailyUpdateEffects
    |> should haveLength 0

let filterMarketUpdateEffects effect =
    match effect with
    | GenreMarketsUpdated _ -> true
    | _ -> false

[<Test>]
let ``tick should update markets every year in the dawn`` () =
    Simulation.tick stateInMidnightBeforeGameStart songStartedEffect
    |> fst
    |> List.filter filterMarketUpdateEffects
    |> should haveLength 1

    Simulation.tick dummyState songStartedEffect
    |> fst
    |> List.filter filterMarketUpdateEffects
    |> should haveLength 0

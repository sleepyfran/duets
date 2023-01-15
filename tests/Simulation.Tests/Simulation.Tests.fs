module Simulation.Tests.Simulation

open FsUnit
open NUnit.Framework
open Test.Common
open Test.Common.Generators

open Entities
open Simulation
open Simulation.Time.AdvanceTime

let state = { dummyState with Today = dummyTodayMiddleOfYear }

let stateInMorning =
    { state with
        Today = state.Today |> Calendar.Transform.changeDayMoment Morning }

let stateInMidnightBeforeGameStart =
    { state with
        Today =
            Calendar.gameBeginning
            |> Calendar.Transform.changeDayMoment Midnight }

let unfinishedSong = (UnfinishedSong dummySong, 10<quality>, 10<quality>)

let songStartedEffect = SongStarted(dummyBand, unfinishedSong)

let effectsWithTimeIncrease =
    [ AlbumRecorded(dummyBand, dummyUnreleasedAlbum), 2<dayMoments>
      ConcertFinished(dummyBand, dummyPastConcert, 0m<dd>), 1<dayMoments>
      songStartedEffect, 1<dayMoments>
      SongImproved(dummyBand, Diff(unfinishedSong, unfinishedSong)),
      1<dayMoments>
      SongPracticed(dummyBand, dummyFinishedSong), 1<dayMoments>
      Wait 1<dayMoments>, 1<dayMoments> ]

let checkTimeIncrease timeIncrease effects =
    effects
    |> should
        contain
        (advanceDayMoment dummyTodayMiddleOfYear timeIncrease |> List.head)

[<Test>]
let ``tick should apply the given effect`` () =
    Simulation.tick state songStartedEffect
    |> fst
    |> should contain songStartedEffect

[<Test>]
let ``tick should not apply the given effect more than once`` () =
    Simulation.tick state songStartedEffect
    |> fst
    |> List.filter (fun effect -> effect = songStartedEffect)
    |> should haveLength 1

[<Test>]
let ``tick should advance time by corresponding effect type`` () =
    effectsWithTimeIncrease
    |> List.iter (fun (effect, timeIncrease) ->
        Simulation.tick state effect |> fst |> checkTimeIncrease timeIncrease)

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
            dummyBand.Id
            (Album.Released.fromUnreleased dummyUnreleasedAlbum dummyToday 1.0)

    Simulation.tick state songStartedEffect
    |> fst
    |> List.filter filterDailyUpdateEffects
    |> should haveLength 2

[<Test>]
let ``tick should update all scheduled concerts every day`` () =
    let state =
        State.generateOne
            { State.defaultOptions with FutureConcertsToGenerate = 10 }

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

[<Test>]
let ``tick should check for failed concerts in every time update`` () =
    let state =
        State.generateOne
            { State.defaultOptions with FutureConcertsToGenerate = 0 }
        |> State.Concerts.addScheduledConcert
            dummyBand
            (ScheduledConcert(dummyConcert, dummyToday))

    let dateAfterConcert =
        Simulation.Queries.Calendar.today state
        |> Calendar.Ops.addDays 31
        |> Calendar.Transform.changeDayMoment Midnight

    let stateAfterConcert =
        TimeAdvanced dateAfterConcert |> State.Root.applyEffect state

    Simulation.tick stateAfterConcert songStartedEffect
    |> fst
    |> List.filter (fun effect ->
        match effect with
        | ConcertCancelled _ -> true
        | _ -> false)
    |> should haveLength 1

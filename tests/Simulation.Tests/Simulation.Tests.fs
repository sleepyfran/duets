module Duets.Simulation.Tests.Simulation

open System
open Aether
open Duets.Data.World
open FsUnit
open NUnit.Framework
open Duets
open Duets.Simulation.Time
open Test.Common
open Test.Common.Generators

open Duets.Entities
open Duets.Entities.Calendar.Shorthands
open Duets.Simulation
open Duets.Data.Careers

let state =
    { dummyState with
        Today = dummyTodayMiddleOfYear }

let stateInMorning =
    { state with
        Today = state.Today |> Calendar.Transform.changeDayMoment Morning }

let stateInMidnightOfNewYear =
    { state with
        Today =
            Spring 1<days> 2023<years>
            |> Calendar.Transform.changeDayMoment Midnight }

let unfinishedSong = Unfinished(dummySong, 10<quality>, 10<quality>)

let songStartedEffect = SongStarted(dummyBand, unfinishedSong)

let checkTimeIncrease timeIncrease effects =
    effects
    |> should
        contain
        (AdvanceTime.advanceDayMoment dummyTodayMiddleOfYear timeIncrease
         |> List.head)

[<Test>]
let ``tick does not try to apply moodlets (breaks) for game created effect``
    ()
    =
    let gameStartedEffect = GameCreated state

    (fun () -> Simulation.tickOne State.empty gameStartedEffect |> ignore)
    |> should not' (throw typeof<Exception>)

[<Test>]
let ``tick should apply the given effect`` () =
    Simulation.tickOne state songStartedEffect
    |> fst
    |> should contain songStartedEffect

[<Test>]
let ``tick should apply multiple given effects`` () =
    Simulation.tickMultiple state [ songStartedEffect; songStartedEffect ]
    |> fst
    |> List.filter (function
        | SongStarted _ -> true
        | _ -> false)
    |> List.length
    |> should equal 2

[<Test>]
let ``tick should gather and apply associated effects`` () =
    let effects =
        AdvanceTime.advanceDayMoment' state 1<dayMoments>
        |> Simulation.tickMultiple state
        |> fst

    effects
    |> List.item 0
    |> should
        equal
        (TimeAdvanced(
            { Year = 2021<years>
              Season = Season.Summer
              Day = 20<days>
              DayMoment = Morning }
        ))

    effects
    |> List.item 1
    |> should
        equal
        (CharacterAttributeChanged(
            dummyCharacter.Id,
            CharacterAttribute.Hunger,
            Diff(100, 85)
        ))

[<Test>]
let ``tick should stop the chain of effects when a BreakChain associated effect is raised``
    ()
    =
    let stateToHospitalize =
        [ CharacterAttributeChanged(
              dummyCharacter.Id,
              CharacterAttribute.Health,
              Diff(100, 1)
          )
          CharacterAttributeChanged(
              dummyCharacter.Id,
              CharacterAttribute.Hunger,
              Diff(100, 1)
          ) ]
        |> State.Root.applyEffects state

    let skippedEffect =
        CharacterAttributeChanged(
            dummyCharacter.Id,
            CharacterAttribute.Drunkenness,
            Diff(0, 10)
        )

    let result =
        [ yield! AdvanceTime.advanceDayMoment' state 1<dayMoments>
          (* Break chain should happen after advancing the moment *)
          skippedEffect ]
        |> Simulation.tickMultiple stateToHospitalize
        |> fst

    result |> should not' (contain skippedEffect)

[<Test>]
let ``tick should not apply the given effect more than once`` () =
    Simulation.tickOne state songStartedEffect
    |> fst
    |> List.filter (fun effect -> effect = songStartedEffect)
    |> should haveLength 1

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

    AdvanceTime.advanceDayMoment' state 1<dayMoments>
    |> Simulation.tickMultiple state
    |> fst
    |> List.filter filterDailyUpdateEffects
    |> should haveLength 2

[<Test>]
let ``tick should update all scheduled concerts every day`` () =
    let state =
        State.generateOne
            { State.defaultOptions with
                FutureConcertsToGenerate = 10 }

    AdvanceTime.advanceDayMoment' state 1<dayMoments>
    |> Simulation.tickMultiple state
    |> fst
    |> List.filter filterDailyUpdateEffects
    |> should haveLength 10

[<Test>]
let ``tick should not update album streams or concerts if morning has passed``
    ()
    =
    Simulation.tickOne stateInMorning songStartedEffect
    |> fst
    |> List.filter filterDailyUpdateEffects
    |> should haveLength 0

let filterMarketUpdateEffects effect =
    match effect with
    | GenreMarketsUpdated _ -> true
    | _ -> false

[<Test>]
let ``tick should update markets every year in the early morning`` () =
    AdvanceTime.advanceDayMoment' stateInMidnightOfNewYear 1<dayMoments>
    |> Simulation.tickMultiple stateInMidnightOfNewYear
    |> fst
    |> List.filter filterMarketUpdateEffects
    |> should haveLength 1

    AdvanceTime.advanceDayMoment' dummyState 1<dayMoments>
    |> Simulation.tickMultiple dummyState
    |> fst
    |> List.filter filterMarketUpdateEffects
    |> should haveLength 0

[<Test>]
let ``tick should check for failed concerts in every time update`` () =
    let state =
        State.generateOne
            { State.defaultOptions with
                FutureConcertsToGenerate = 0 }
        |> State.Concerts.addScheduledConcert
            dummyBand
            (ScheduledConcert(dummyConcert, dummyToday))

    let dateAfterConcert =
        Simulation.Queries.Calendar.today state
        |> Calendar.Ops.addDays 31<days>
        |> Calendar.Transform.changeDayMoment Midnight

    let stateAfterConcert =
        TimeAdvanced dateAfterConcert |> State.Root.applyEffect state

    AdvanceTime.advanceDayMoment' stateAfterConcert 1<dayMoments>
    |> Simulation.tickMultiple stateAfterConcert
    |> fst
    |> List.filter (fun effect ->
        match effect with
        | ConcertCancelled _ -> true
        | _ -> false)
    |> should haveLength 1

[<Test>]
let ``tick should update turn time with the total minutes spent in the tick``
    ()
    =
    Simulation.tickOne state songStartedEffect
    |> snd
    |> Optic.get Lenses.State.turnMinutes_
    |> should equal 120<minute>

[<Test>]
let ``tick should advance day moment if the effects triggered enough time`` () =
    let effects, stateAfterTick =
        Simulation.tickMultiple state [ songStartedEffect; songStartedEffect ]

    effects
    |> List.find (function
        | TimeAdvanced _ -> true
        | _ -> false)
    |> should
        equal
        (TimeAdvanced(
            { Year = 2021<years>
              Season = Season.Summer
              Day = 20<days>
              DayMoment = Morning }
        ))

    stateAfterTick
    |> Optic.get Lenses.State.turnMinutes_
    |> should equal 0<minute>

[<Test>]
let ``tick should keep leftover time after advancing day moment if effects triggered enough time``
    ()
    =
    let effects, stateAfterTick =
        Simulation.tickMultiple
            state
            [ songStartedEffect; songStartedEffect; songStartedEffect ]

    effects
    |> List.find (function
        | TimeAdvanced _ -> true
        | _ -> false)
    |> should
        equal
        (TimeAdvanced(
            { Year = 2021<years>
              Season = Season.Summer
              Day = 20<days>
              DayMoment = Morning }
        ))

    stateAfterTick
    |> Optic.get Lenses.State.turnMinutes_
    |> should equal 120<minute>

[<Test>]
let ``ticks that include effects spanning multiple day moments get applied correctly``
    ()
    =
    let baristaManagerCareer = BaristaCareer.stages |> List.last

    let effects, stateAfterTick =
        CareerShiftPerformed(
            { Id = Barista
              CurrentStage = baristaManagerCareer
              Location = (Prague, "", Ids.Cafe.cafe) },
            4<dayMoments>,
            0.0m<dd>
        )
        |> Simulation.tickOne state

    effects
    |> List.find (function
        | TimeAdvanced _ -> true
        | _ -> false)
    |> should
        equal
        (TimeAdvanced(
            { Year = 2021<years>
              Season = Season.Summer
              Day = 20<days>
              DayMoment = Morning }
        ))

    stateAfterTick
    |> Optic.get Lenses.State.turnMinutes_
    |> should equal 0<minute>

module Duets.Simulation.Tests.Events.Character

open FsUnit
open NUnit.Framework
open Test.Common
open Test.Common.Generators

open Duets.Entities
open Duets.Simulation

let lowCharacterHealthEffect state =
    let character = Queries.Characters.playableCharacter state

    CharacterAttributeChanged(
        character.Id,
        CharacterAttribute.Health,
        Diff(15, 5)
    )

[<Test>]
let ``tick of low character health should generate health depleted`` () =
    let state = State.generateOne State.defaultOptions

    Simulation.tickOne state (lowCharacterHealthEffect state)
    |> fst
    |> List.item 1
    |> should be (ofCase <@ CharacterHealthDepleted @>)

[<Test>]
let ``tick of low character health should hospitalize character`` () =
    let state = State.generateOne State.defaultOptions

    Simulation.tickOne state (lowCharacterHealthEffect state)
    |> fst
    |> List.item 2
    |> should be (ofCase <@ CharacterHospitalized @>)

[<Test>]
let ``tick of low character health during concert should cancel concert`` () =
    let state =
        State.generateOne
            { State.defaultOptions with FutureConcertsToGenerate = 0 }

    let stateOnConcert =
        Situations.inConcert
            { Concert = dummyConcert
              Events = []
              Points = 0<quality> }
        |> State.Root.applyEffect state

    Simulation.tickOne stateOnConcert (lowCharacterHealthEffect state)
    |> fst
    |> List.item 1
    |> should be (ofCase <@ ConcertCancelled @>)

[<Test>]
let ``tick of low character health advances one week`` () =
    let state = State.generateOne State.defaultOptions

    let oneWeekLater = Calendar.gameBeginning |> Calendar.Ops.addDays 7

    Simulation.tickOne state (lowCharacterHealthEffect state)
    |> fst
    |> should contain (TimeAdvanced oneWeekLater)

let private assertAttributeChanged attribute amount effect =
    let state = State.generateOne State.defaultOptions

    let character = Queries.Characters.playableCharacter state

    Simulation.tickOne state effect
    |> fst
    |> List.choose (fun effect ->
        match effect with
        | CharacterAttributeChanged(characterId, attr, diff) ->
            Some(characterId, attr, diff)
        | _ -> None)
    |> List.head
    |> fun (characterId, attr, Diff(_, currentAmount)) ->
        characterId |> should equal character.Id
        attr |> should equal attribute
        currentAmount |> should equal amount

[<Test>]
let ``tick of song improved should decrease energy`` () =
    SongImproved(dummyBand, Diff(dummyUnfinishedSong, dummyUnfinishedSong))
    |> assertAttributeChanged CharacterAttribute.Energy 80

[<Test>]
let ``tick of song started should decrease energy`` () =
    SongStarted(dummyBand, dummyUnfinishedSong)
    |> assertAttributeChanged CharacterAttribute.Energy 80

[<Test>]
let ``tick of song practiced should decrease energy`` () =
    SongPracticed(dummyBand, dummyFinishedSong)
    |> assertAttributeChanged CharacterAttribute.Energy 90

[<Test>]
let ``tick of passing time should decrease character's drunkenness`` () =
    let state = State.generateOne State.defaultOptions

    let character = Queries.Characters.playableCharacter state

    let stateAfterGettingDrunk =
        Character.Attribute.add character CharacterAttribute.Drunkenness 15
        |> State.Root.applyEffects state

    let character = Queries.Characters.playableCharacter stateAfterGettingDrunk

    let oneDayMomentLater =
        Calendar.gameBeginning |> Calendar.Transform.changeDayMoment Morning

    Simulation.tickOne stateAfterGettingDrunk (TimeAdvanced oneDayMomentLater)
    |> fst
    |> should
        contain
        (CharacterAttributeChanged(
            character.Id,
            CharacterAttribute.Drunkenness,
            Diff(15, 10)
        ))


[<Test>]
let ``tick of passing time should decrease character's health when passing 85 in drunkenness``
    ()
    =
    let state = State.generateOne State.defaultOptions

    let character = Queries.Characters.playableCharacter state

    let stateAfterGettingDrunk =
        Character.Attribute.add character CharacterAttribute.Drunkenness 95
        |> State.Root.applyEffects state

    let character = Queries.Characters.playableCharacter stateAfterGettingDrunk

    let oneDayMomentLater =
        Calendar.gameBeginning |> Calendar.Transform.changeDayMoment Morning

    let expectedHealth = 100 + Config.LifeSimulation.drunkHealthReduceRate

    Simulation.tickOne stateAfterGettingDrunk (TimeAdvanced oneDayMomentLater)
    |> fst
    |> should
        contain
        (CharacterAttributeChanged(
            character.Id,
            CharacterAttribute.Health,
            Diff(100, expectedHealth)
        ))

let private testConcertEffect effect moodIncrease =
    let state =
        State.generateOne
            { State.defaultOptions with
                CharacterMoodMin = 50
                CharacterMoodMax = 50 }

    let character = Queries.Characters.playableCharacter state

    Simulation.tickOne state effect
    |> fst
    |> should
        contain
        (CharacterAttributeChanged(
            character.Id,
            CharacterAttribute.Mood,
            Diff(50, 50 + moodIncrease)
        ))

let private testConcertFinishedEffect concertQuality =
    ConcertFinished(
        dummyBand,
        PerformedConcert(dummyConcert, concertQuality),
        0m<dd>
    )
    |> testConcertEffect

[<Test>]
let ``tick of ConcertCancelled should greatly decrease character's mood`` () =
    let effect =
        ConcertCancelled(
            dummyBand,
            FailedConcert(dummyConcert, BandDidNotMakeIt)
        )

    testConcertEffect effect Config.LifeSimulation.Mood.concertFailIncrease

[<Test>]
let ``tick of ConcertFinished with a bad concert should decrease character's mood``
    ()
    =
    testConcertFinishedEffect
        20<quality>
        Config.LifeSimulation.Mood.concertPoorResultIncrease

[<Test>]
let ``tick of ConcertFinished with an okay concert should increase character's mood``
    ()
    =
    testConcertFinishedEffect
        50<quality>
        Config.LifeSimulation.Mood.concertNormalResultIncrease

[<Test>]
let ``tick of ConcertFinished with a good concert should decrease character's mood``
    ()
    =
    testConcertFinishedEffect
        80<quality>
        Config.LifeSimulation.Mood.concertGoodResultIncrease

[<Test>]
let ``tick of BandFansChanged updates the character's fame to half of the estimated fame of the band``
    ()
    =
    let state =
        dummyState
        |> State.Characters.setAttribute
            dummyCharacter.Id
            CharacterAttribute.Fame
            8

    let effect = BandFansChanged(dummyBand, Diff(5000, 10000))

    Simulation.tickOne state effect
    |> fst
    |> should
        contain
        (CharacterAttributeChanged(
            dummyCharacter.Id,
            CharacterAttribute.Fame,
            Diff(8, 16)
        ))

module Duets.Simulation.Tests.EffectModifiers.Moodlet

open FsUnit
open NUnit.Framework
open Duets
open Test.Common
open Test.Common.Generators

open Duets.Entities
open Duets.Simulation

let private moodletsWithNotInspired =
    [ Moodlet.create
          MoodletType.NotInspired
          dummyToday
          MoodletExpirationTime.Never ]
    |> Set.ofList

let private moodletsWithJetLagged =
    [ Moodlet.create
          MoodletType.JetLagged
          dummyToday
          MoodletExpirationTime.Never ]
    |> Set.ofList

let private notInspiredState =
    dummyState
    |> State.Characters.setMoodlets dummyCharacter.Id moodletsWithNotInspired

let private jetLaggedState =
    dummyState
    |> State.Characters.setMoodlets dummyCharacter.Id moodletsWithJetLagged

let private unfinishedSong = Unfinished(dummySong, 100<quality>, 20<quality>)
let private songStartedEffect = SongStarted(dummyBand, unfinishedSong)

let private songImprovedEffect =
    SongImproved(
        dummyBand,
        Diff(unfinishedSong, Unfinished(dummySong, 100<quality>, 40<quality>))
    )

// =============================================================================
// JetLagged
// =============================================================================

[<Test>]
let ``tick of CharacterAttributeChanged does not change if character does not have moodlet``
    ()
    =
    let effect =
        CharacterAttributeChanged(
            dummyCharacter.Id,
            CharacterAttribute.Energy,
            Diff(50, 100)
        )

    let characterAttributeChangedEffects =
        Simulation.tickOne dummyState effect
        |> fst
        |> List.filter (function
            | CharacterAttributeChanged _ -> true
            | _ -> false)

    characterAttributeChangedEffects |> should haveLength 1

    characterAttributeChangedEffects |> List.head |> should equal effect

[<Test>]
let ``tick of CharacterAttributeChanged does not change if character has JetLagged moodlet but the attribute is not energy``
    ()
    =
    let effect =
        CharacterAttributeChanged(
            dummyCharacter.Id,
            CharacterAttribute.Fame,
            Diff(50, 100)
        )

    let characterAttributeChangedEffects =
        Simulation.tickOne jetLaggedState effect
        |> fst
        |> List.filter (function
            | CharacterAttributeChanged _ -> true
            | _ -> false)

    characterAttributeChangedEffects |> should haveLength 1

    characterAttributeChangedEffects |> List.head |> should equal effect

[<Test>]
let ``tick of CharacterAttributeChanged does not change if character has JetLagged moodlet but energy is decreasing``
    ()
    =
    let effect =
        CharacterAttributeChanged(
            dummyCharacter.Id,
            CharacterAttribute.Energy,
            Diff(100, 50)
        )

    let characterAttributeChangedEffects =
        Simulation.tickOne jetLaggedState effect
        |> fst
        |> List.filter (function
            | CharacterAttributeChanged _ -> true
            | _ -> false)

    characterAttributeChangedEffects |> should haveLength 1

    characterAttributeChangedEffects |> List.head |> should equal effect

[<Test>]
let ``tick of CharacterAttributeChanged reduces the change by 50% when character has JetLagged moodlet and energy is increasing``
    ()
    =
    let effect =
        CharacterAttributeChanged(
            dummyCharacter.Id,
            CharacterAttribute.Energy,
            Diff(50, 100)
        )

    let characterAttributeChangedEffects =
        Simulation.tickOne jetLaggedState effect
        |> fst
        |> List.filter (function
            | CharacterAttributeChanged _ -> true
            | _ -> false)

    characterAttributeChangedEffects |> should haveLength 1

    let effect = characterAttributeChangedEffects |> List.head

    match effect with
    | CharacterAttributeChanged(id, attribute, Diff(before, after)) ->
        id |> should equal dummyCharacter.Id
        attribute |> should equal CharacterAttribute.Energy
        before |> should equal 50
        after |> should equal 75
    | _ -> failwith "Unexpected effect"

// =============================================================================
// NotInspired
// =============================================================================

[<Test>]
let ``tick of SongStarted does not change if character does not have moodlet``
    ()
    =
    let songStartedEffects =
        Simulation.tickOne dummyState songStartedEffect
        |> fst
        |> List.filter (function
            | SongStarted _ -> true
            | _ -> false)

    songStartedEffects |> should haveLength 1
    songStartedEffects |> List.head |> should equal songStartedEffect

[<Test>]
let ``tick of SongStarted reduces the score by 75% when not inspired`` () =
    let songStartedEffects =
        Simulation.tickOne notInspiredState songStartedEffect
        |> fst
        |> List.filter (function
            | SongStarted _ -> true
            | _ -> false)

    songStartedEffects |> should haveLength 1
    let effect = songStartedEffects |> List.head

    match effect with
    | SongStarted(band, Unfinished(song, maxQuality, currentQuality)) ->
        band |> should equal dummyBand
        song |> should equal dummySong
        maxQuality |> should equal 25<quality>
        currentQuality |> should equal 5<quality>
    | _ -> failwith "Unexpected effect"

[<Test>]
let ``tick of SongImproved reduces the improved score by 75% when not inspired``
    ()
    =
    let songImprovedEffects =
        Simulation.tickOne notInspiredState songImprovedEffect
        |> fst
        |> List.filter (function
            | SongImproved _ -> true
            | _ -> false)

    songImprovedEffects |> should haveLength 1
    let effect = songImprovedEffects |> List.head

    match effect with
    | SongImproved(band,
                   Diff(before, Unfinished(song, maxQuality, currentQuality))) ->
        band |> should equal dummyBand
        before |> should equal unfinishedSong
        song |> should equal dummySong
        maxQuality |> should equal 100<quality>
        currentQuality |> should equal 10<quality>
    | _ -> failwith "Unexpected effect"

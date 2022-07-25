module Simulation.Tests.Events.Character

open FsUnit
open NUnit.Framework
open Test.Common
open Test.Common.Generators

open Entities
open Simulation

let lowCharacterHealthEffect state =
    let character =
        Queries.Characters.playableCharacter state

    CharacterAttributeChanged(character, CharacterAttribute.Health, 5)

[<Test>]
let ``tick of low character health should generate health depleted`` () =
    let state =
        State.generateOne State.defaultOptions

    Simulation.tick state (lowCharacterHealthEffect state)
    |> fst
    |> List.item 1
    |> should be (ofCase <@ CharacterHealthDepleted @>)

[<Test>]
let ``tick of low character health should hospitalize character`` () =
    let state =
        State.generateOne State.defaultOptions

    Simulation.tick state (lowCharacterHealthEffect state)
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

    Simulation.tick stateOnConcert (lowCharacterHealthEffect state)
    |> fst
    |> List.item 1
    |> should be (ofCase <@ ConcertCancelled @>)

[<Test>]
let ``tick of low character health advances one week`` () =
    let state =
        State.generateOne State.defaultOptions

    let oneWeekLater =
        Calendar.gameBeginning |> Calendar.Ops.addDays 7

    Simulation.tick state (lowCharacterHealthEffect state)
    |> fst
    |> should contain (TimeAdvanced oneWeekLater)

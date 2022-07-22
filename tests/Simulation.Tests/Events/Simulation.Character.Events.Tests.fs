module Simulation.Tests.Events.Character

open FsUnit
open NUnit.Framework
open Test.Common
open Test.Common.Generators

open Entities
open Simulation
open Simulation.Time.AdvanceTime

[<Test>]
let ``tick of CharacterHealthDepleted should hospitalize character`` () =
    let state =
        State.generateOne State.defaultOptions

    let effect =
        Queries.Characters.playableCharacter state
        |> CharacterHealthDepleted

    Simulation.tick state effect
    |> fst
    |> List.item 1
    |> should be (ofCase <@ CharacterHospitalized @>)

[<Test>]
let ``tick of CharacterHealthDepleted during concert should cancel concert``
    ()
    =
    let state =
        State.generateOne
            { State.defaultOptions with FutureConcertsToGenerate = 0 }

    let stateOnConcert =
        Situations.inConcert
            { Concert = dummyConcert
              Events = []
              Points = 0<quality> }
        |> State.Root.applyEffect state

    let effect =
        Queries.Characters.playableCharacter state
        |> CharacterHealthDepleted

    Simulation.tick stateOnConcert effect
    |> fst
    |> List.item 1
    |> should be (ofCase <@ ConcertCancelled @>)

[<Test>]
let ``tick of CharacterHospitalized advances one week`` () =
    let state =
        State.generateOne State.defaultOptions

    let effect =
        Queries.Characters.playableCharacter state
        |> CharacterHealthDepleted

    let oneWeekLater =
        Calendar.gameBeginning |> Calendar.Ops.addDays 7

    Simulation.tick state effect
    |> fst
    |> should contain (TimeAdvanced oneWeekLater)

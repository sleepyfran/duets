module Duets.Simulation.Tests.Skills.ImproveSkills

open NUnit.Framework
open FsUnit
open Test.Common

open Duets.Entities
open Duets
open Duets.Simulation.Skills.ImproveSkills

let unwrapDiff (Diff (_, (_, level))) = level

let unwrapSkillImproved effect =
    match effect with
    | SkillImproved (_, diff) -> unwrapDiff diff
    | _ -> invalidOp "Unexpected effect"

let matchesImprovementValue effects =
    let compositionLevel =
        Seq.item 0 effects |> unwrapSkillImproved

    let genreLevel =
        Seq.item 1 effects |> unwrapSkillImproved

    let instrumentLevel =
        Seq.item 2 effects |> unwrapSkillImproved

    compositionLevel |> should equal 1
    genreLevel |> should equal 1
    instrumentLevel |> should equal 1

[<Test>]
let ``should increase skills by one if random is more than 50`` () =
    [ 60; 55; 51; 100; 78; 80 ]
    |> List.iter
        (fun randomValue ->
            staticRandom randomValue
            |> Simulation.RandomGen.change

            Composition.improveBandSkillsChance dummyBand dummyState
            |> matchesImprovementValue)

[<Test>]
let ``should not increase skills that is already at a 100`` () =
    staticRandom 80 |> Simulation.RandomGen.change

    let state =
        addSkillTo
            dummyCharacter
            (Skill.create SkillId.Composition, 100)
            dummyState

    Composition.improveBandSkillsChance dummyBand state
    |> should haveLength 2

    let state =
        addSkillTo
            dummyCharacter
            (dummyBand.Genre |> SkillId.Genre |> Skill.create, 100)
            state

    Composition.improveBandSkillsChance dummyBand state
    |> should haveLength 1

[<Test>]
let ``should not increase skills if random is less than 50`` () =
    [ 1; 24; 12; 30; 45; 49; 50 ]
    |> List.iter
        (fun randomValue ->
            staticRandom randomValue
            |> Simulation.RandomGen.change

            Composition.improveBandSkillsChance dummyBand dummyState
            |> should haveLength 0)

module Simulation.Tests.Skills.ImproveSkills

open NUnit.Framework
open FsUnit
open Test.Common

open Entities
open Simulation.Skills.ImproveSkills

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
let ImproveBandSkillsAfterComposingShouldImproveRandomValuesIfRandomIsMoreThan50
    ()
    =
    [ 60; 55; 51; 100; 78; 80 ]
    |> List.iter
        (fun randomValue ->
            improveBandSkillsAfterComposing'
                (fun _ _ -> randomValue)
                dummyState
                dummyBand
            |> matchesImprovementValue)

[<Test>]
let ImproveBandSkillsAfterComposingShouldNotImproveIfRandomIsLessThanOrEqualTo50
    ()
    =
    [ 1; 24; 12; 30; 45; 49; 50 ]
    |> List.iter
        (fun randomValue ->
            improveBandSkillsAfterComposing'
                (fun _ _ -> randomValue)
                dummyState
                dummyBand
            |> should haveLength 0)

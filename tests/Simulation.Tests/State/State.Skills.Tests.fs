module Duets.Simulation.Tests.State.Skills



open FsUnit
open NUnit.Framework
open Test.Common

open Duets.Common
open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Queries

let skill = Skill.createWithLevel SkillId.Composition 10

let querySkills character state =
    Skills.characterSkillsWithLevel state character

[<Test>]
let ``SkillImproved should add skill if not present`` () =
    let skills =
        SkillImproved(dummyCharacter, Diff(skill, skill))
        |> State.Root.applyEffect dummyState
        |> querySkills dummyCharacter.Id

    skills |> should haveCount 1
    skills |> Map.head |> should equal skill

[<Test>]
let ``SkillImproved should add skill even if character is not present in the map``
    ()
    =
    let madeUpCharacter =
        Character.from
            "Made Up"
            Male
            (Calendar.Ops.addYears -25<years> Calendar.gameBeginning)

    let skills =
        SkillImproved(madeUpCharacter, Diff(skill, skill))
        |> State.Root.applyEffect dummyState
        |> querySkills madeUpCharacter.Id

    skills |> should haveCount 1
    skills |> Map.head |> should equal skill

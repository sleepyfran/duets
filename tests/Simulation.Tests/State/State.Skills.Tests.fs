module Simulation.State.Tests.Skills


open FsUnit
open NUnit.Framework
open Test.Common

open Aether
open Common
open Entities
open Simulation
open Simulation.Queries

let skill =
    Skill.createWithLevel SkillId.Composition 10

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
            (Calendar.Ops.addYears -25 Calendar.gameBeginning)

    let skills =
        SkillImproved(madeUpCharacter, Diff(skill, skill))
        |> State.Root.applyEffect dummyState
        |> querySkills madeUpCharacter.Id

    skills |> should haveCount 1
    skills |> Map.head |> should equal skill

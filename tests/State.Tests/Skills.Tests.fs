module State.Tests.Skills

open FsUnit
open NUnit.Framework
open Test.Common

open Aether
open Common
open Entities
open Simulation.Queries

let skill =
    Skill.createWithLevel SkillId.Composition 10

let querySkills character =
    State.Root.get ()
    |> fun state -> Skills.characterSkillsWithLevel state character

[<SetUp>]
let Setup () = initState ()

[<Test>]
let ``SkillImproved should add skill if not present`` () =
    State.Root.apply
    <| SkillImproved(dummyCharacter, Diff(skill, skill))

    let skills = querySkills dummyCharacter.Id

    skills |> should haveCount 1
    skills |> Map.head |> should equal skill

[<Test>]
let ``SkillImproved should add skill even if character is not present in the map``
    ()
    =
    let madeUpCharacter =
        Character.from "Made Up" 25 Male |> Result.unwrap

    State.Root.apply
    <| SkillImproved(madeUpCharacter, Diff(skill, skill))

    let skills = querySkills madeUpCharacter.Id

    skills |> should haveCount 1
    skills |> Map.head |> should equal skill

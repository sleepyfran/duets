module State.Tests.BandManagement

open FsUnit
open NUnit.Framework
open Test.Common

open Common
open Entities
open Simulation.Queries

[<SetUp>]
let Setup () = Common.initState ()

let hiredMember =
    let character =
        Character.from "Another" 18 Other |> Result.unwrap

    Band.Member.from character Guitar dummyToday

let memberSkills =
    [ (Skill.createWithLevel SkillId.Composition 10) ]

let hireMember () =
    State.Root.apply
    <| MemberHired(dummyBand, hiredMember, memberSkills)

[<Test>]
let ``MemberHired should add member to band`` () =
    hireMember ()

    State.Root.get ()
    |> Bands.currentBandMembersWithoutPlayableCharacter
    |> List.head
    |> should equal hiredMember

[<Test>]
let ``MemberHired should add skills to member's character`` () =
    hireMember ()

    let characterSkills =
        Skills.characterSkillsWithLevel
            (State.Root.get ())
            hiredMember.Character.Id

    characterSkills
    |> Map.head
    |> should equal (List.head memberSkills)

[<Test>]
let ``MemberFired should remove band member and add past member`` () =
    hireMember ()

    State.Root.get ()
    |> Bands.currentBandMembersWithoutPlayableCharacter
    |> should haveLength 1

    let firedMember =
        Band.PastMember.fromMember hiredMember dummyToday

    State.Root.apply
    <| MemberFired(dummyBand, hiredMember, firedMember)

    let state = State.Root.get ()

    state
    |> Bands.currentBandMembersWithoutPlayableCharacter
    |> should haveLength 0

    state
    |> Bands.pastBandMembers
    |> List.head
    |> should equal firedMember

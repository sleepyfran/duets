module Duets.Simulation.Tests.State.BandManagement



open FsUnit
open NUnit.Framework
open Test.Common

open Duets.Common
open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Queries

let hiredCharacter =
    Character.from
        "Another"
        Other
        (Calendar.Ops.addYears -18<years> Calendar.gameBeginning)

let hiredMember = Band.Member.from hiredCharacter.Id Guitar dummyToday

let memberSkills = [ (Skill.createWithLevel SkillId.Composition 10) ]

let hireMember () =
    MemberHired(dummyBand, hiredCharacter, hiredMember, memberSkills)
    |> State.Root.applyEffect dummyState

[<Test>]
let ``MemberHired should add character to character map`` () =
    let stateAfterHiring = hireMember ()

    Characters.find stateAfterHiring hiredMember.CharacterId
    |> should equal hiredCharacter

[<Test>]
let ``MemberHired should add member to band`` () =
    hireMember ()
    |> Bands.currentBandMembersWithoutPlayableCharacter
    |> List.head
    |> should equal hiredMember

[<Test>]
let ``MemberHired should add skills to member's character`` () =
    let state = hireMember ()

    let characterSkills =
        Skills.characterSkillsWithLevel state hiredMember.CharacterId

    characterSkills |> Map.head |> should equal (List.head memberSkills)

[<Test>]
let ``MemberFired should remove band member and add past member`` () =
    let state = hireMember ()

    state
    |> Bands.currentBandMembersWithoutPlayableCharacter
    |> should haveLength 1

    let firedMember = Band.PastMember.fromMember hiredMember dummyToday

    let state =
        MemberFired(dummyBand, hiredMember, firedMember)
        |> State.Root.applyEffect state

    state
    |> Bands.currentBandMembersWithoutPlayableCharacter
    |> should haveLength 0

    state |> Bands.pastBandMembers |> List.head |> should equal firedMember

module State.Tests.BandManagement

open FsUnit
open NUnit.Framework
open Test.Common

open Common
open Entities
open Simulation.Bands.Queries

[<SetUp>]
let Setup () = Common.initState ()

let hiredMember =
    let character =
        Character.from "Another" 18 Other |> Result.unwrap

    Band.Member.from character Guitar dummyToday

let hireMember () =
    State.Root.apply
    <| MemberHired(dummyBand, hiredMember)

[<Test>]
let MemberHiredShouldAddMember () =
    hireMember ()

    State.Root.get ()
    |> currentBandMembersWithoutPlayableCharacter
    |> List.head
    |> should equal hiredMember

[<Test>]
let MemberFiredShouldRemoveMemberAndAddToPastMember () =
    hireMember ()

    State.Root.get ()
    |> currentBandMembersWithoutPlayableCharacter
    |> should haveLength 1

    let firedMember =
        Band.PastMember.fromMember hiredMember dummyToday

    State.Root.apply
    <| MemberFired(dummyBand, hiredMember, firedMember)

    let state = State.Root.get ()

    state
    |> currentBandMembersWithoutPlayableCharacter
    |> should haveLength 0

    state
    |> pastBandMembers
    |> List.head
    |> should equal firedMember

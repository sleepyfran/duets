module Simulation.Tests.Bands.FireMember

open Test.Common
open NUnit.Framework
open FsUnit

open Common
open Entities
open Simulation.Bands.Members

let bandMember =
    let hiredCharacter =
        Character.from "Test" 18 Other |> Result.unwrap

    Band.Member.from hiredCharacter Guitar dummyToday

let state =
    dummyState |> addMember dummyBand bandMember

[<Test>]
let FireMemberFailsIfGivenMemberIsPlayableCharacter () =
    let playableMember =
        Band.Member.from dummyCharacter Guitar dummyToday

    fireMember state dummyBand playableMember
    |> Result.unwrapError
    |> should be (ofCase <@ AttemptToFirePlayableCharacter @>)

[<Test>]
let FireMemberGeneratesMemberFiredEffect () =
    fireMember state dummyBand bandMember
    |> Result.unwrap
    |> should
        be
        (ofCase
            <@ MemberFired(
                dummyBand,
                bandMember,
                Band.PastMember.fromMember bandMember dummyToday
            ) @>)

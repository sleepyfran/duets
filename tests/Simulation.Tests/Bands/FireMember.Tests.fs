module Duets.Simulation.Tests.Bands.FireMember

open Test.Common
open NUnit.Framework
open FsUnit

open Duets.Common
open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Bands.Members

let bandMember =
    let hiredCharacter =
        Character.from
            "Test"
            Other
            (Calendar.Ops.addYears -28 Calendar.gameBeginning)

    Band.Member.from hiredCharacter.Id Guitar dummyToday

let state = dummyState |> State.Bands.addMember dummyBand bandMember

[<Test>]
let FireMemberFailsIfGivenMemberIsPlayableCharacter () =
    let playableMember = Band.Member.from dummyCharacter.Id Guitar dummyToday

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
            <@
                MemberFired(
                    dummyBand,
                    bandMember,
                    Band.PastMember.fromMember bandMember dummyToday
                )
            @>)

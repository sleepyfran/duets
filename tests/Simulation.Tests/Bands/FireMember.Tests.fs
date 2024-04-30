module Duets.Simulation.Tests.Bands.FireMember

open Test.Common
open NUnit.Framework
open FsUnit

open Duets.Common
open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Bands.Members

let hiredCharacter =
    Character.from
        "Test"
        Other
        (Calendar.Ops.addYears -28 Calendar.gameBeginning)

let bandMember = Band.Member.from hiredCharacter.Id Guitar dummyToday

let state =
    dummyState
    |> State.Characters.add hiredCharacter
    |> State.Bands.addMember dummyBand bandMember

[<Test>]
let FireMemberFailsIfGivenMemberIsPlayableCharacter () =
    let playableMember = Band.Member.from dummyCharacter.Id Guitar dummyToday

    RehearsalRoomFireMember
        {| Band = dummyBand
           CurrentMember = playableMember |}
    |> runFailingAction state
    |> should be (ofCase <@ CannotFirePlayableCharacter @>)

[<Test>]
let FireMemberGeneratesMemberFiredEffect () =
    RehearsalRoomFireMember
        {| Band = dummyBand
           CurrentMember = bandMember |}
    |> runSucceedingAction state
    |> fst
    |> List.head
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

module Duets.Simulation.Tests.Bands.HireMember

open Test.Common
open NUnit.Framework
open FsUnit

open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Bands.Members

let instrument =
    Instrument.createInstrument Guitar

let skillLevel = 50

let state =
    dummyState
    |> addSkillTo
        dummyCharacter
        (Skill.createWithLevel SkillId.Composition skillLevel)
    |> addSkillTo
        dummyCharacter
        (Skill.createWithLevel (SkillId.Genre dummyBand.Genre) skillLevel)

let assertOnMembers assertion =
    membersForHire state dummyBand instrument.Type
    |> Seq.take 20
    |> Seq.iter assertion

let memberForHire =
    membersForHire state dummyBand instrument.Type
    |> Seq.take 1
    |> Seq.head

let hiredMember =
    Band.Member.fromMemberForHire memberForHire dummyToday

[<Test>]
let MembersForHireShouldExposeMembersOfGivenInstrument () =
    assertOnMembers (fun m -> m.Role |> should equal Guitar)

[<Test>]
let MembersForHireShouldExposeMembersWithAtLeastThreeSkills () =
    assertOnMembers (fun m -> m.Skills |> should haveLength 3)

[<Test>]
let MembersForHireShouldExposeMembersWithSkillLevelAroundBandsAverage () =
    let assertLevelRange =
        fun (_, level) ->
            level
            |> should be (lessThanOrEqualTo (skillLevel + 5))

            level
            |> should be (greaterThanOrEqualTo (skillLevel - 5))

    assertOnMembers (fun m -> List.iter assertLevelRange m.Skills)

[<Test>]
let MembersForHireShouldExposeMembersWithAgeAroundBandsAverage () =
    let characterAge =
        Queries.Characters.ageOf state dummyCharacter

    let assertAgeRange =
        fun age ->
            age
            |> should be (lessThanOrEqualTo (characterAge + 5))

            age
            |> should be (greaterThanOrEqualTo (characterAge - 5))

    assertOnMembers (fun m ->
        Queries.Characters.ageOf state m.Character
        |> assertAgeRange)

[<Test>]
let HireMemberShouldGeneratedHiredMemberEffect () =
    hireMember state dummyBand memberForHire
    |> should
        be
        (ofCase
            <@ MemberHired(
                dummyBand,
                memberForHire.Character,
                hiredMember,
                memberForHire.Skills
            ) @>)

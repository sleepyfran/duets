module Simulation.Tests.Bands.HireMember

open Test.Common
open NUnit.Framework
open FsUnit

open Entities
open Simulation.Bands.Members
open Simulation.Calendar.Queries

let instrument = Instrument.createInstrument Guitar
let skillLevel = 50

let state =
  dummyState
  |> addSkillTo
       dummyCharacter
       (Skill.createWithLevel SkillId.Composition skillLevel)
  |> addSkillTo
       dummyCharacter
       (Skill.createWithLevel (Genre dummyBand.Genre) skillLevel)

let assertOnMembers assertion =
  membersForHire state dummyBand instrument
  |> Seq.take 20
  |> Seq.iter assertion

let memberForHire =
  membersForHire state dummyBand instrument
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
  let characterAge = dummyCharacter.Age

  let assertAgeRange =
    fun age ->
      age
      |> should be (lessThanOrEqualTo (characterAge + 5))

      age
      |> should be (greaterThanOrEqualTo (characterAge - 5))

  assertOnMembers (fun m -> assertAgeRange m.Character.Age)

[<Test>]
let HireMemberShouldGeneratedHiredMemberEffect () =
  hireMember state dummyBand memberForHire
  |> should be (ofCase <@ MemberHired(dummyBand, hiredMember) @>)

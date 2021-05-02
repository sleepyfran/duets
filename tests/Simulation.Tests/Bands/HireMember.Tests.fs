module Simulation.Tests.Bands.HireMember

open Test.Common
open NUnit.Framework
open FsUnit

open Entities
open Simulation.Bands.Members
open Simulation.Calendar.Queries

let instrument = Instrument.createInstrument Guitar
let skillLevel = 50

let assertOnMembers assertion =
  membersForHire (currentBand ()) instrument
  |> Seq.take 20
  |> Seq.iter assertion

[<SetUp>]
let Setup () =
  initStateWithDummies ()
  let character = currentCharacter ()
  addSkillTo character (Skill.createWithLevel SkillId.Composition skillLevel)

  addSkillTo
    character
    (Skill.createWithLevel (Genre dummyBand.Genre) skillLevel)

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
  let characterAge = (currentCharacter ()).Age

  let assertAgeRange =
    fun age ->
      age
      |> should be (lessThanOrEqualTo (characterAge + 5))

      age
      |> should be (greaterThanOrEqualTo (characterAge - 5))

  assertOnMembers (fun m -> assertAgeRange m.Character.Age)

[<Test>]
let HireMemberShouldAddMemberToBand () =
  membersForHire (currentBand ()) instrument
  |> Seq.take 1
  |> Seq.head
  |> hireMember

  let band = currentBand ()
  band.Members |> should haveLength 2

[<Test>]
let HireMemberShouldAddMemberToBandWithTodayAsSinceDate () =
  membersForHire (currentBand ()) instrument
  |> Seq.take 1
  |> Seq.head
  |> hireMember

  let band = currentBand ()

  List.last band.Members
  |> fun m -> m.Since
  |> should equal (today ())

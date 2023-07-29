module Simulation.Tests.Careers.JobBoard

open FsCheck
open FsUnit
open NUnit.Framework
open Test.Common.Generators

open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Careers

let private stateWithoutSkills =
    let state = State.generateOne State.defaultOptions

    { state with
        CharacterSkills = Map.empty }

let private baristaSkill = Skill.create SkillId.Barista
let private bartendingSkill = Skill.create SkillId.Bartending

let private character = Queries.Characters.playableCharacter stateWithoutSkills

[<Test>]
let ``availableJobsInCurrentCity returns jobs in initial career stage if character has no skills associated with it``
    ()
    =
    JobBoard.availableJobsInCurrentCity stateWithoutSkills CareerId.Barista
    |> List.iter (fun job ->
        job.CurrentStage.Id |> should equal (CareerStageId 0uy))

[<Test>]
let ``availableJobsInCurrentCity returns jobs with the appropriate career stage for the skills of the character``
    ()
    =
    [ (CareerId.Barista, baristaSkill, (8, 12), 1uy)
      (CareerId.Barista, baristaSkill, (38, 42), 2uy)
      (CareerId.Barista, baristaSkill, (58, 62), 3uy)
      (CareerId.Bartender, bartendingSkill, (8, 12), 1uy)
      (CareerId.Bartender, bartendingSkill, (38, 42), 2uy)
      (CareerId.Bartender, bartendingSkill, (58, 62), 3uy) ]
    |> List.iter (fun (careerId, skill, skillLevelRange, expectedStage) ->
        let skillLevel =
            Gen.choose skillLevelRange |> Gen.sample 1 1 |> List.head

        let state =
            stateWithoutSkills
            |> State.Skills.add character.Id (skill, skillLevel)

        JobBoard.availableJobsInCurrentCity state careerId
        |> List.iter (fun job ->
            job.CurrentStage.Id |> should equal (CareerStageId expectedStage)))

module Duets.Simulation.Tests.Events.Skill

open FsUnit
open NUnit.Framework
open Test.Common

open Duets.Data
open Duets.Entities
open Duets.Simulation

let baristaJob =
    { Id = Barista
      CurrentStage = Careers.BaristaCareer.stages |> List.head
      Location = dummyCity.Id, dummyPlace.Id
      Schedule = JobSchedule.Free 2<dayMoments>
      ShiftAttributeEffect = [] }

let bartenderJob =
    { Id = Bartender
      CurrentStage = Careers.BartenderCareer.stages |> List.head
      Location = dummyCity.Id, dummyPlace.Id
      Schedule = JobSchedule.Free 2<dayMoments>
      ShiftAttributeEffect = [] }

let baristaSkill = Skill.create SkillId.Barista
let bartendingSkill = Skill.create SkillId.Bartending

let shiftPerformedEffect job = CareerShiftPerformed(job, 100m<dd>)

[<Test>]
let ``tick of CareerShiftPerformed should improve career skill if chance of 25% succeeds``
    ()
    =
    [ 1..25 ]
    |> List.iter (fun randomValue ->
        staticRandom randomValue |> RandomGen.change

        Simulation.tickOne dummyState (shiftPerformedEffect baristaJob)
        |> fst
        |> List.item 1
        |> should be (ofCase <@ SkillImproved @>))

[<Test>]
let ``tick of CareerShiftPerformed does not improve career skill if chance of 25% fails``
    ()
    =
    [ 26..100 ]
    |> List.iter (fun randomValue ->
        staticRandom randomValue |> RandomGen.change

        Simulation.tickOne dummyState (shiftPerformedEffect baristaJob)
        |> fst
        |> should haveLength 1)

[<Test>]
let ``tick of CareerShiftPerformed with successful chance improves job career by 1``
    ()
    =
    staticRandom 5 |> RandomGen.change

    [ baristaJob, baristaSkill; bartenderJob, bartendingSkill ]
    |> List.iter (fun (job, expectedSkill) ->
        Simulation.tickOne dummyState (shiftPerformedEffect job)
        |> fst
        |> List.item 1
        |> should
            equal
            (SkillImproved(
                dummyCharacter,
                Diff((expectedSkill, 0), (expectedSkill, 1))
            )))

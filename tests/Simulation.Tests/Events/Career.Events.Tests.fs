module Duets.Simulation.Tests.Events.Career

#nowarn "25"

open Duets.Data.World
open FsUnit
open NUnit.Framework
open Test.Common

open Duets.Data
open Duets.Entities
open Duets.Simulation

let baristaJob =
    { Id = Barista
      CurrentStage = Careers.BaristaCareer.stages |> List.head
      Location = dummyCity.Id, dummyPlace.Id, Ids.Common.cafe }

let bartenderJob =
    { Id = Bartender
      CurrentStage = Careers.BartenderCareer.stages |> List.head
      Location = dummyCity.Id, dummyPlace.Id, Ids.Common.cafe }

let baristaSkill = Skill.create SkillId.Barista
let bartendingSkill = Skill.create SkillId.Bartending

let shiftPerformedEffect job =
    CareerShiftPerformed(job, 2<dayMoments>, 100m<dd>)

(* --------- Skill improvement. --------- *)

[<Test>]
let ``tick of CareerShiftPerformed should improve career skill if chance of 25% succeeds``
    ()
    =
    [ 1..25 ]
    |> List.iter (fun randomValue ->
        use _ = changeToStaticRandom randomValue

        Simulation.tickOne dummyState (shiftPerformedEffect baristaJob)
        |> fst
        |> List.filter (function
            | SkillImproved _ -> true
            | _ -> false)
        |> should haveLength 1)

[<Test>]
let ``tick of CareerShiftPerformed does not improve career skill if chance of 25% fails``
    ()
    =
    [ 26..100 ]
    |> List.iter (fun randomValue ->
        use _ = changeToStaticRandom randomValue

        Simulation.tickOne dummyState (shiftPerformedEffect baristaJob)
        |> fst
        |> List.filter (function
            | SkillImproved _ -> true
            | _ -> false)
        |> should haveLength 0)

[<Test>]
let ``tick of CareerShiftPerformed with successful chance improves job career by 1``
    ()
    =
    use _ = changeToStaticRandom 5

    [ baristaJob, baristaSkill; bartenderJob, bartendingSkill ]
    |> List.iter (fun (job, expectedSkill) ->
        Simulation.tickOne dummyState (shiftPerformedEffect job)
        |> fst
        |> List.filter (function
            | SkillImproved _ -> true
            | _ -> false)
        |> List.head
        |> fun (SkillImproved(_, diff)) ->
            diff |> should equal (Diff((expectedSkill, 0), (expectedSkill, 1))))

(* --------- Promotions --------- *)

let private promotionEffects =
    function
    | CareerPromoted _ -> true
    | _ -> false

[<Test>]
let ``tick of CareerShiftPerformed does not grant promotion if 10% chance fails``
    ()
    =
    [ 10..100 ]
    |> List.iter (fun randomValue ->
        use _ = changeToStaticRandom randomValue

        Simulation.tickOne dummyState (shiftPerformedEffect baristaJob)
        |> fst
        |> List.filter promotionEffects
        |> should haveLength 0)

[<Test>]
let ``tick of CareerShiftPerformed does not grant promotion if character does not have enough skills for next stage even if 10% chance succeeds``
    ()
    =
    [ 0..9 ]
    |> List.iter (fun randomValue ->
        use _ = changeToStaticRandom randomValue

        Simulation.tickOne dummyState (shiftPerformedEffect baristaJob)
        |> fst
        |> List.filter promotionEffects
        |> should haveLength 0)

[<Test>]
let ``tick of CareerShiftPerformed grants promotion if character has enough skills for next stage and 10% chance succeeds``
    ()
    =
    let stateWithSkill =
        dummyState
        |> State.Skills.add dummyCharacter.Id (Skill.create SkillId.Barista, 20)

    [ 0..9 ]
    |> List.iter (fun randomValue ->
        use _ = changeToStaticRandom randomValue

        Simulation.tickOne stateWithSkill (shiftPerformedEffect baristaJob)
        |> fst
        |> List.filter promotionEffects
        |> should haveLength 1)

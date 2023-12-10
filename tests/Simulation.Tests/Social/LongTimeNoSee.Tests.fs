module Duets.Simulation.Tests.Social.LongTimeNoSee

open FsUnit
open NUnit.Framework
open Test.Common

open Duets.Common
open Duets.Entities
open Duets.Simulation

let private createRelationshipWithLevel lastInteractionTime level =
    { Character = dummyCharacter2.Id
      MeetingPlace = Prague, ""
      LastIterationDate = lastInteractionTime
      RelationshipType = Acquaintance
      Level = level }

let createStateWithRelationship lastInteractionTime =
    { dummyState with
        Relationships =
            [ (dummyCharacter2.Id,
               createRelationshipWithLevel
                   lastInteractionTime
                   10<relationshipLevel>) ]
            |> Map.ofList }

[<Test>]
let ``does nothing if character has no relationships`` () =
    Social.LongTimeNoSee.applyIfNeeded dummyState |> should haveLength 0

[<Test>]
let ``does nothing if the last interaction was less than 14 days ago`` () =
    [ 0..13 ]
    |> List.iter (fun daysSince ->
        dummyToday
        |> Calendar.Ops.addDays -daysSince
        |> createStateWithRelationship
        |> Social.LongTimeNoSee.applyIfNeeded
        |> should haveLength 0)

[<Test>]
let ``reduces relationship level by 5 and sets last interaction time to today if interaction was more than 14 days ago``
    ()
    =
    let effects =
        dummyToday
        |> Calendar.Ops.addDays -15
        |> createStateWithRelationship
        |> Social.LongTimeNoSee.applyIfNeeded

    let expectedRelationship =
        createRelationshipWithLevel dummyToday 5<relationshipLevel>

    effects |> should haveLength 1

    effects
    |> List.head
    |> should
        equal
        (RelationshipChanged(dummyCharacter2.Id, Some expectedRelationship))

[<Test>]
let ``gets applied every day in the morning`` () =
    let state =
        dummyToday |> Calendar.Ops.addDays -15 |> createStateWithRelationship

    dummyToday
    |> Calendar.Transform.changeDayMoment Morning
    |> TimeAdvanced
    |> Simulation.tickOne state
    |> fst
    |> List.filter (function
        | RelationshipChanged _ -> true
        | _ -> false)
    |> should haveLength 1

module Duets.Simulation.Tests.State.Relationships

open System
open FsUnit
open NUnit.Framework
open Test.Common

open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Queries

let npc =
    Character.from
        "Npc"
        Other
        (Calendar.gameBeginning |> Calendar.Ops.addYears -20)

let relationship: Relationship =
    { Character = npc.Id
      Level = 10<relationshipLevel>
      MeetingCity = Prague
      RelationshipType = Friend
      LastIterationDate = dummyToday }

[<Test>]
let ``RelationshipChanged adds relationship to character ID map`` () =
    let state =
        RelationshipChanged(npc, Prague, Some relationship)
        |> State.Root.applyEffect dummyState

    Relationship.withCharacter npc.Id state |> should equal (Some relationship)

[<Test>]
let ``RelationshipChanged adds relationship to meeting city ID map`` () =
    let state =
        RelationshipChanged(npc, Prague, Some relationship)
        |> State.Root.applyEffect dummyState

    let cityRelationships = Relationship.fromCity Prague state
    cityRelationships |> should haveLength 1
    cityRelationships |> List.head |> should equal relationship

[<Test>]
let ``RelationshipChanged adds character to character map`` () =
    let state =
        RelationshipChanged(npc, Prague, Some relationship)
        |> State.Root.applyEffect dummyState

    Characters.find state npc.Id |> _.Id |> should equal npc.Id

[<Test>]
let ``RelationshipChanged removes relationship from character ID map`` () =
    let state =
        RelationshipChanged(npc, Prague, Some relationship)
        |> State.Root.applyEffect dummyState

    let state =
        RelationshipChanged(npc, Prague, None) |> State.Root.applyEffect state

    Relationship.withCharacter npc.Id state |> should equal None

[<Test>]
let ``RelationshipChanged removes relationship from meeting city ID map`` () =
    let state =
        RelationshipChanged(npc, Prague, Some relationship)
        |> State.Root.applyEffect dummyState

    let state =
        RelationshipChanged(npc, Prague, None) |> State.Root.applyEffect state

    let cityRelationships = Relationship.fromCity Prague state
    cityRelationships |> should haveLength 0

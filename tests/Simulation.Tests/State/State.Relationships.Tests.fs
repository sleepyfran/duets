module Duets.Simulation.Tests.State.Relationships

open FsUnit
open NUnit.Framework
open Test.Common
open Test.Common.Generators

open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Queries

let relationship: Relationship =
    { Character = dummyCharacter.Id
      Level = 10<relationshipLevel>
      MeetingCity = Prague
      RelationshipType = Acquaintance
      LastIterationDate = dummyToday }

[<Test>]
let ``RelationshipChanged adds relationship to character ID map`` () =
    let state =
        RelationshipChanged(dummyCharacter.Id, Prague, Some relationship)
        |> State.Root.applyEffect dummyState

    Relationship.withCharacter dummyCharacter.Id state
    |> should equal (Some relationship)

[<Test>]
let ``RelationshipChanged adds relationship to meeting city ID map`` () =
    let state =
        RelationshipChanged(dummyCharacter.Id, Prague, Some relationship)
        |> State.Root.applyEffect dummyState

    let cityRelationships = Relationship.fromCity Prague state
    cityRelationships |> should haveLength 1
    cityRelationships |> List.head |> should equal relationship

[<Test>]
let ``RelationshipChanged removes relationship from character ID map`` () =
    let state =
        RelationshipChanged(dummyCharacter.Id, Prague, Some relationship)
        |> State.Root.applyEffect dummyState

    let state =
        RelationshipChanged(dummyCharacter.Id, Prague, None)
        |> State.Root.applyEffect state

    Relationship.withCharacter dummyCharacter.Id state |> should equal None

[<Test>]
let ``RelationshipChanged removes relationship from meeting city ID map`` () =
    let state =
        RelationshipChanged(dummyCharacter.Id, Prague, Some relationship)
        |> State.Root.applyEffect dummyState

    let state =
        RelationshipChanged(dummyCharacter.Id, Prague, None)
        |> State.Root.applyEffect state

    let cityRelationships = Relationship.fromCity Prague state
    cityRelationships |> should haveLength 0

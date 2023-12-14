module Duets.Simulation.Tests.Migrations.BandMemberRelationships

open FsUnit
open NUnit.Framework
open Test.Common

open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Migrations

let private createMember (character: Character) =
    Band.Member.from character.Id InstrumentType.Guitar dummyToday

let skill = Skill.create SkillId.Composition, 100

let members =
    [ dummyCharacter2, createMember dummyCharacter2
      dummyCharacter3, createMember dummyCharacter3 ]

let hireEffects =
    members
    |> List.map (fun (character, bandMember) ->
        MemberHired(dummyBand, character, bandMember, [ skill ]))

[<Test>]
let ``applying migration adds a relationship for each member of the band`` () =
    let initialState =
        hireEffects
        |> List.fold
            (fun state effect -> effect |> State.Root.applyEffect state)
            dummyState

    // Double check that we didn't add them already.
    initialState |> Queries.Relationship.all |> should haveCount 0

    let migratedState = Migrations.apply initialState

    migratedState |> Queries.Relationship.all |> should haveCount members.Length

[<Test>]
let ``applying migration does nothing if relationships were already added`` () =
    let _, initialState = hireEffects |> Simulation.tickMultiple dummyState

    let currentRelationships = initialState |> Queries.Relationship.all

    // Verify that we added them already.
    currentRelationships |> should haveCount members.Length

    let migratedState = Migrations.apply initialState

    migratedState
    |> Queries.Relationship.all
    |> should equal currentRelationships

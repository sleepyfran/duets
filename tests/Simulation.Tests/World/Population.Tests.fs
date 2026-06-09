module Duets.Simulation.Tests.World.Population

open FsUnit
open NUnit.Framework
open Test.Common

open Duets.Entities
open Duets.Simulation

let private knownCharacter = dummyCharacter3

let private airport =
    Queries.World.placesByTypeInCity Prague PlaceTypeIndex.Airport |> List.head

let private hotel =
    Queries.World.placesByTypeInCity Prague PlaceTypeIndex.Hotel |> List.head

let private home =
    Queries.World.placesByTypeInCity Prague PlaceTypeIndex.Home |> List.head

let private bar =
    Queries.World.placesByTypeInCity Prague PlaceTypeIndex.Bar |> List.head

let private extractCharacters =
    function
    | WorldPeopleInCurrentRoomChanged(characters) -> characters
    | _ -> failwith "Unexpected effect"

[<SetUp>]
let setup = RandomGen.reset

[<Test>]
let ``generateForPlace does not generate anyone at home`` () =
    World.Population.generateForPlace Prague home RoomType.Kitchen dummyState
    |> extractCharacters
    |> should haveLength 0

[<Test>]
let ``generateForPlace generates people depending on place type`` () =
    World.Population.generateForPlace Prague airport RoomType.Lobby dummyState
    |> extractCharacters
    |> _.Length
    |> should be (inRange 0 10)

    World.Population.generateForPlace Prague hotel RoomType.Lobby dummyState
    |> extractCharacters
    |> _.Length
    |> should be (inRange 1 5)

[<Test>]
let ``generateForPlace assigns fixed and weighted NPC room goals`` () =
    use _ =
        [ (* Number of people being generated. *)
          yield 10
          (* Birthday generation for each random NPC. *)
          for _ in 1..10 do
              yield 18
              yield 0 ]
        |> changeToOrderedRandom

    let generatedPeople =
        World.Population.generateForPlace Prague bar RoomType.Bar dummyState
        |> extractCharacters

    generatedPeople |> should haveLength 10

    let goalsByCount = generatedPeople |> List.countBy _.Goal |> Map.ofList

    goalsByCount
    |> should equal (Map.ofList
                         [ NpcRoomGoal.Working(PlayableWork Bartender), 1
                           NpcRoomGoal.Customer, 9 ])

[<Test>]
let ``generateForPlace should add known people if character has relationships and does not add duplicates``
    ()
    =
    (* Force adding known people. *)
    use _ =
        [
          (* Number of people being generated. *)
          5
          (* Number to force to pull from known NPCs. *)
          1
          (* Sampled index from the known NPCs. *)
          0
          (* Samples for the NPC random gen. *)
          yield! [ 1..10 ] ]
        |> changeToOrderedRandom

    let stateWithRelationship =
        RelationshipChanged(
            knownCharacter,
            Prague,
            Some(
                { Character = knownCharacter.Id
                  Level = 10<relationshipLevel>
                  MeetingCity = Prague
                  RelationshipType = Friend
                  DiscoveredTraits = Set.empty
                  LastIterationDate = dummyToday }
            )
        )
        |> State.Root.applyEffect dummyState

    World.Population.generateForPlace
        Prague
        airport
        RoomType.SecurityControl
        stateWithRelationship
    |> extractCharacters
    |> List.filter (fun { Npc = character } -> character.Id = knownCharacter.Id)
    |> should haveLength 1

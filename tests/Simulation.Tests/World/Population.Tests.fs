module Duets.Simulation.Tests.World.Population

open FsUnit
open NUnit.Framework
open Test.Common

open Duets.Data.World
open Duets.Entities
open Duets.Simulation

let private knownCharacter = dummyCharacter3

let private firstPlace placeType =
    Queries.World.placesByTypeInCity Prague placeType |> List.head

let private airport = firstPlace PlaceTypeIndex.Airport
let private concertSpace = firstPlace PlaceTypeIndex.ConcertSpace
let private home = firstPlace PlaceTypeIndex.Home
let private hotel = firstPlace PlaceTypeIndex.Hotel
let private rehearsalSpace = firstPlace PlaceTypeIndex.RehearsalSpace

let private extractCharacters =
    function
    | WorldPeopleInCurrentRoomChanged(characters) -> characters
    | _ -> failwith "Unexpected effect"

let private generate place roomId state =
    World.Population.generateForRoom Prague place roomId state
    |> extractCharacters

let private stateAt dayMoment =
    let date = dummyToday |> Calendar.Transform.changeDayMoment dayMoment

    { dummyState with
        Today = date }

[<SetUp>]
let setup = RandomGen.reset

[<Test>]
let ``generateForRoom does not generate anyone at home`` () =
    generate home Ids.Home.livingRoom dummyState |> should haveLength 0

[<Test>]
let ``generateForRoom does not generate anyone in a hotel bedroom`` () =
    generate hotel Ids.Home.bedroom dummyState |> should haveLength 0

[<Test>]
let ``generateForRoom does not generate anyone in the backstage`` () =
    generate concertSpace Ids.ConcertSpace.backstage dummyState
    |> should haveLength 0

[<Test>]
let ``generateForRoom generates people depending on room type`` () =
    generate airport Ids.Common.lobby dummyState
    |> _.Length
    |> should be (inRange 0 4)

    generate hotel Ids.Common.lobby dummyState
    |> _.Length
    |> should be (inRange 0 4)

[<Test>]
let ``generateForRoom generates fewer people on the street at midnight`` () =
    let street = firstPlace PlaceTypeIndex.Street

    for _ in 1..20 do
        generate street street.Rooms.StartingNode (stateAt Midnight)
        |> _.Length
        |> should be (inRange 0 3)

[<Test>]
let ``generateForRoom only adds band members to the rehearsal room`` () =
    let bandMemberIds =
        Queries.Bands.currentBandMembersWithoutPlayableCharacter dummyState
        |> List.map _.CharacterId

    let isBandMember (character: Character) =
        bandMemberIds |> List.contains character.Id

    generate rehearsalSpace (Ids.RehearsalRoom.room 1) dummyState
    |> List.filter isBandMember
    |> should haveLength bandMemberIds.Length

    generate rehearsalSpace Ids.Common.lobby dummyState
    |> List.filter isBandMember
    |> should haveLength 0

[<Test>]
let ``generateForRoom should add known people if character has relationships and does not add duplicates``
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
                  LastIterationDate = dummyToday }
            )
        )
        |> State.Root.applyEffect dummyState

    generate airport Ids.Common.lobby stateWithRelationship
    |> List.filter (fun character -> character.Id = knownCharacter.Id)
    |> should haveLength 1

module Duets.Simulation.Tests.Events.Moodlets.JetLagged

open Duets.Data.World
open FsUnit
open NUnit.Framework
open Test.Common

open Duets.Entities
open Duets.Simulation

let private worldMoveEffect prevCity currCity =
    let queryPlace cityId =
        Queries.World.placesByTypeInCity cityId PlaceTypeIndex.Airport
        |> List.head

    let prevCityPlace = queryPlace prevCity
    let currCityPlace = queryPlace currCity


    WorldMoveToPlace(
        Diff(
            (prevCity, prevCityPlace.Id, Ids.Common.lobby),
            (currCity, currCityPlace.Id, Ids.Common.lobby)
        )
    )

[<Test>]
let ``tick of WorldMoveTo does not apply any extra effects if the difference in timezones is less than 4 hours``
    ()
    =
    [ London, Prague; Madrid, Prague; NewYork, LosAngeles ]
    |> List.iter (fun (prevCity, currCity) ->
        Simulation.tickOne dummyState (worldMoveEffect prevCity currCity)
        |> fst
        |> List.filter (function
            | CharacterMoodletsChanged _ -> true
            | _ -> false)
        |> should haveLength 0)

[<Test>]
let ``tick of song finished should apply JetLagged moodlet if the cities are more than 4 timezones apart``
    ()
    =
    [ London, NewYork; NewYork, London; LosAngeles, London; London, LosAngeles ]
    |> List.iter (fun (prevCity, currCity) ->
        let moodletEffect =
            Simulation.tickOne dummyState (worldMoveEffect prevCity currCity)
            |> fst
            |> List.filter (function
                | CharacterMoodletsChanged _ -> true
                | _ -> false)
            |> List.head

        match moodletEffect with
        | CharacterMoodletsChanged(_, Diff(prevMoodlet, currMoodlet)) ->
            prevMoodlet |> should haveCount 0
            currMoodlet |> should haveCount 1

            let moodlet = currMoodlet |> Set.toList |> List.head

            moodlet.MoodletType
            |> should be (ofCase <@ MoodletType.JetLagged @>)

            moodlet.StartedOn |> should equal dummyToday
        | _ -> failwith "Unexpected effect")

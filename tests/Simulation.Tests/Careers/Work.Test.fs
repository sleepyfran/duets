module Duets.Simulation.Tests.Careers.Work

open FsCheck
open FsUnit
open Fugit.Months
open NUnit.Framework
open NUnit.Framework.Internal.Execution
open Test.Common
open Test.Common.Generators

open Duets.Data
open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Careers

let private place =
    Queries.World.placesByTypeInCity Prague PlaceTypeIndex.Cafe |> List.head

let private job =
    { Id = Barista
      CurrentStage = (Careers.BaristaCareer.stages |> List.head)
      Location = Prague, place.Id }

let morningTime = August 21 2023 |> Calendar.Transform.changeDayMoment Morning

let private state =
    State.generateOne State.defaultOptions |> State.Calendar.setTime morningTime

[<TestFixture>]
type ``When place is not near closing time``() =
    [<Test>]
    member _.``work pays the full payment for all the day moments worked``() =
        let effect =
            WorkShift job
            |> runSucceedingAction state
            |> fst
            |> List.filter (function
                | MoneyEarned _ -> true
                | _ -> false)
            |> List.head

        match effect with
        | MoneyEarned(_, Incoming(_, amount)) -> amount |> should equal 14m<dd>
        | _ -> failwith "Unexpected effect"

    [<Test>]
    member _.``work passes the number of shift day moments specified in the career stage when place is not near closing time``
        ()
        =
        WorkShift job
        |> runSucceedingAction state
        |> fst
        |> List.filter (function
            | TimeAdvanced _ -> true
            | _ -> false)
        |> should haveLength 2

[<TestFixture>]
type ``When place is near closing time``() =
    let eveningTime = morningTime |> Calendar.Transform.changeDayMoment Evening

    let stateInEvening = state |> State.Calendar.setTime eveningTime

    [<Test>]
    member _.``work pays only the worked day moments until the place will close``
        ()
        =
        let effect =
            WorkShift job
            |> runSucceedingAction stateInEvening
            |> fst
            |> List.filter (function
                | MoneyEarned _ -> true
                | _ -> false)
            |> List.head

        match effect with
        | MoneyEarned(_, Incoming(_, amount)) -> amount |> should equal 7m<dd>
        | _ -> failwith "Unexpected effect"

    [<Test>]
    member _.``work passes only the number of day moments until the place will close``
        ()
        =
        WorkShift job
        |> runSucceedingAction stateInEvening
        |> fst
        |> List.filter (function
            | TimeAdvanced _ -> true
            | _ -> false)
        |> should haveLength 1

module Duets.Simulation.Tests.Careers.Work

#nowarn "25"

open FsCheck
open FsUnit
open NUnit.Framework
open Test.Common.Generators

open Duets.Data
open Duets.Entities
open Duets.Entities.Calendar.Shorthands
open Duets.Simulation
open Duets.Simulation.Careers

let private place =
    Queries.World.placesByTypeInCity Prague PlaceTypeIndex.Cafe |> List.head

let private job =
    { Id = Barista
      CurrentStage = (Careers.BaristaCareer.stages |> List.head)
      Location = Prague, place.Id }

let morningTime =
    Summer 21<days> 2023<years> |> Calendar.Transform.changeDayMoment Morning

let private state =
    State.generateOne State.defaultOptions |> State.Calendar.setTime morningTime

[<TestFixture>]
type ``When place is not near closing time``() =
    [<Test>]
    member _.``work pays the full payment for all the day moments worked``() =
        let effect =
            Work.workShift state job
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
        Work.workShift state job
        |> List.filter (function
            | CareerShiftPerformed _ -> true
            | _ -> false)
        |> List.head
        |> fun (CareerShiftPerformed(_, shiftDuration, _)) ->
            shiftDuration |> should equal 2<dayMoments>


[<TestFixture>]
type ``When place is near closing time``() =
    let eveningTime = morningTime |> Calendar.Transform.changeDayMoment Evening

    let stateInEvening = state |> State.Calendar.setTime eveningTime

    [<Test>]
    member _.``work pays only the worked day moments until the place will close``
        ()
        =
        let effect =
            Work.workShift stateInEvening job
            |> Simulation.tickMultiple stateInEvening
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
        Work.workShift stateInEvening job
        |> Simulation.tickMultiple stateInEvening
        |> fst
        |> List.filter (function
            | TimeAdvanced _ -> true
            | _ -> false)
        |> should haveLength 1

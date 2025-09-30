module Duets.Simulation.Tests.Careers.Work

#nowarn "25"

open System
open Duets.Data.World
open Duets.Simulation.Careers.Work
open FsCheck
open FsUnit
open NUnit.Framework
open Test.Common.Generators

open Duets.Common
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
      Location = Prague, place.Id, Ids.Common.cafe }

let morningTime =
    Summer 21<days> 2023<years> |> Calendar.Transform.changeDayMoment Morning

let private state =
    State.generateOne State.defaultOptions
    |> State.Calendar.setTime morningTime
    |> State.World.move Prague place.Id Ids.Common.cafe

[<TestFixture>]
type ``When place is not near closing time``() =
    [<Test>]
    member _.``work pays the full payment for all the day moments worked``() =
        let effect =
            Work.workShift state job
            |> Result.unwrap
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
        |> Result.unwrap
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
            |> Result.unwrap
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
        |> Result.unwrap
        |> Simulation.tickMultiple stateInEvening
        |> fst
        |> List.filter (function
            | TimeAdvanced _ -> true
            | _ -> false)
        |> should haveLength 1

[<TestFixture>]
type ``When place is closed``() =
    let nightTime = morningTime |> Calendar.Transform.changeDayMoment Night

    let stateAtNight = state |> State.Calendar.setTime nightTime

    [<Test>]
    member _.``work returns an error``() =
        workShift stateAtNight job
        |> Result.unwrapError
        |> should equal WorkshiftError.AttemptedToWorkDuringClosingTime

[<TestFixture>]
type ``When job has fixed schedule``() =
    // Day 21 is Sunday, day 22 is Monday, day 23 is Tuesday, day 24 is Wednesday
    let sundayTime = morningTime // Day 21 = Sunday
    let mondayTime = sundayTime |> Calendar.Query.nextN 7 // Day 22 = Monday  
    let tuesdayTime = mondayTime |> Calendar.Query.nextN 7 // Day 23 = Tuesday
    let wednesdayTime = tuesdayTime |> Calendar.Query.nextN 7 // Day 24 = Wednesday
    
    // Create a job with fixed schedule for Monday and Wednesday only, during Morning and Afternoon
    let fixedScheduleJob =
        { Id = Barista
          CurrentStage = 
            { (Careers.BaristaCareer.stages |> List.head) with
                Schedule = JobSchedule.Fixed([DayOfWeek.Monday; DayOfWeek.Wednesday], [DayMoment.Morning; DayMoment.Afternoon], 2<dayMoments>) }
          Location = Prague, place.Id, Ids.Cafe.cafe }
    
    let stateOnMonday = state |> State.Calendar.setTime mondayTime
    let stateOnTuesday = state |> State.Calendar.setTime tuesdayTime
    let stateOnWednesday = state |> State.Calendar.setTime wednesdayTime

    [<Test>]
    member _.``work succeeds on scheduled work day (Monday)``() =
        workShift stateOnMonday fixedScheduleJob
        |> Result.isOk
        |> should be True

    [<Test>]
    member _.``work succeeds on scheduled work day (Wednesday)``() =
        workShift stateOnWednesday fixedScheduleJob
        |> Result.isOk
        |> should be True

    [<Test>]
    member _.``work returns error on non-scheduled work day (Tuesday)``() =
        workShift stateOnTuesday fixedScheduleJob
        |> Result.unwrapError
        |> should equal WorkshiftError.AttemptedToWorkOnNonScheduledDay

    [<Test>]
    member _.``work succeeds on scheduled work day and day moment (Monday Morning)``() =
        workShift stateOnMonday fixedScheduleJob
        |> Result.isOk
        |> should be True

    [<Test>]
    member _.``work succeeds on scheduled work day and day moment (Wednesday Afternoon)``() =
        let afternoonWednesdayTime = wednesdayTime |> Calendar.Transform.changeDayMoment Afternoon
        let stateOnWednesdayAfternoon = state |> State.Calendar.setTime afternoonWednesdayTime
        
        workShift stateOnWednesdayAfternoon fixedScheduleJob
        |> Result.isOk
        |> should be True

    [<Test>]
    member _.``work returns error on scheduled work day but wrong day moment (Monday Midday)``() =
        let middayMondayTime = mondayTime |> Calendar.Transform.changeDayMoment Midday
        let stateOnMondayMidday = state |> State.Calendar.setTime middayMondayTime
        
        workShift stateOnMondayMidday fixedScheduleJob
        |> Result.unwrapError
        |> should equal WorkshiftError.AttemptedToWorkOnNonScheduledDay

    [<Test>]
    member _.``work returns error on scheduled work day but wrong day moment (Wednesday Midday)``() =
        let middayWednesdayTime = wednesdayTime |> Calendar.Transform.changeDayMoment Midday
        let stateOnWednesdayMidday = state |> State.Calendar.setTime middayWednesdayTime
        
        workShift stateOnWednesdayMidday fixedScheduleJob
        |> Result.unwrapError
        |> should equal WorkshiftError.AttemptedToWorkOnNonScheduledDay

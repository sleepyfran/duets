module Duets.Simulation.Tests.Time.AdvanceTime_Tests

open System
open FsUnit
open Fugit.Shorthand
open NUnit.Framework

open Duets.Entities
open Duets.Simulation.Queries
open Duets.Simulation.Time.AdvanceTime

let private unwrapTime =
    function
    | TimeAdvanced time -> time
    | _ -> failwith "Unexpected effect"

let dateMatches (expected: DateTime) time =
    let actual = unwrapTime time
    expected.Year |> should equal actual.Year
    expected.Month |> should equal actual.Month
    expected.Day |> should equal actual.Day
    TimeAdvanced actual

let hourMatches (hour: int) time =
    let date = unwrapTime time
    date.Hour |> should equal hour

[<Test>]
let ``advanceDayMoment should return next day moment`` () =
    let effects = advanceDayMoment Calendar.gameBeginning 1<dayMoments>

    effects |> should haveLength 1

    List.head effects |> dateMatches Calendar.gameBeginning |> hourMatches 10

[<Test>]
let ``advanceDayMoment should roll over next day if current day moment is midnight``
    ()
    =
    let initialDate =
        Calendar.gameBeginning |> Calendar.Transform.changeDayMoment Night

    let effects = advanceDayMoment initialDate 1<dayMoments>

    effects |> should haveLength 1

    List.head effects
    |> dateMatches (Calendar.gameBeginning + oneDay)
    |> hourMatches 0

[<Test>]
let ``advanceDayMoment 2 should return 2 day moments later`` () =
    let effects = advanceDayMoment Calendar.gameBeginning 2<dayMoments>

    effects |> should haveLength 2

    effects
    |> List.rev
    |> List.head
    |> dateMatches Calendar.gameBeginning
    |> hourMatches 14

module Simulation.Tests.Time.AdvanceTime_Tests

open System
open FsUnit
open Fugit.Shorthand
open NUnit.Framework

open Entities
open Simulation.Queries
open Simulation.Time.AdvanceTime

let dateMatches (expected: DateTime) (actual: DateTime) =
    expected.Year |> should equal actual.Year
    expected.Month |> should equal actual.Month
    expected.Day |> should equal actual.Day
    actual

let hourMatches (hour: int) (date: DateTime) = date.Hour |> should equal hour

[<Test>]
let ``advanceTimeOnce should return next day moment`` () =
    advanceTimeOnce Calendar.gameBeginning
    |> dateMatches Calendar.gameBeginning
    |> hourMatches 10

[<Test>]
let ``advanceTimeOnce should roll over next day if current day moment is midnight``
    ()
    =
    Calendar.gameBeginning
    |> Calendar.withDayMoment Midnight
    |> advanceTimeOnce
    |> dateMatches (Calendar.gameBeginning + oneDay)
    |> hourMatches 6

[<Test>]
let ``advanceTimeTimes 2 should return 2 day moments later`` () =
    advanceTimeTimes Calendar.gameBeginning 2
    |> dateMatches Calendar.gameBeginning
    |> hourMatches 14
[<Test>]
let ``advanceTimeTimes should roll over to next day if times surpasses midnight`` () =
   let duskDayMoment = Calendar.gameBeginning |> Calendar.withDayMoment Dusk
   
   advanceTimeTimes duskDayMoment 3
   |> dateMatches (Calendar.gameBeginning + oneDay)
   |> hourMatches 6
